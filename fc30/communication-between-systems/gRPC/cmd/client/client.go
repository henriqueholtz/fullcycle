package main

import (
	"context"
	"fmt"
	"io"
	"log"
	"time"

	"github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/gRPC/pb"
	"google.golang.org/grpc"
)

func main() {
	connection, err := grpc.Dial("localhost:50051", grpc.WithInsecure())
	if err != nil {
		log.Fatalf("Couldn't connect to gRPC Server: %w", err)
	}
	defer connection.Close()

	client:= pb.NewUserServiceClient(connection)
	// AddUser(client)
	// AddUserVerbose(client)
	AddUsers(client)

}

func AddUser(client pb.UserServiceClient) {
	req := &pb.User{
		Id: "0",
		Name: "João",
		Email: "j@j.com",
	}

	res, err := client.AddUser(context.Background(), req)
	if err != nil {
		log.Fatalf("Couldn't make gRPC request: %w", err)
	}

	fmt.Println(res)
}

func AddUserVerbose(client pb.UserServiceClient) {
	req := &pb.User{
		Id: "0",
		Name: "João",
		Email: "j@j.com",
	}

	responseStream, err := client.AddUserVerbose(context.Background(), req)
	if err != nil {
		log.Fatalf("Couldn't make gRPC request: %w", err)
	}

	for {
		stream, err := responseStream.Recv()
		
		if err == io.EOF {
			break // finished
		}
		if err != nil {
			log.Fatalf("Couldn't receive the message: %w", err)
		}

		fmt.Println("Status: ", stream.Status, " - ", stream.GetUser())
	}
}

func AddUsers(client pb.UserServiceClient) {
	reqs := []*pb.User{
		&pb.User{
			Id: "h1",
			Name: "Henrique 1",
			Email: "henrique@1.com",
		},
		&pb.User{
			Id: "h2",
			Name: "Henrique 2",
			Email: "henrique@2.com",
		},
		&pb.User{
			Id: "h3",
			Name: "Henrique 3",
			Email: "henrique@3.com",
		},
		&pb.User{
			Id: "h4",
			Name: "Henrique 4",
			Email: "henrique@4.com",
		},
		&pb.User{
			Id: "h5",
			Name: "Henrique 5",
			Email: "henrique@5.com",
		},
	}

	stream, err := client.AddUsers(context.Background())
	if err != nil {
		log.Fatalf("Error creating request: %w", err)
	}

	for _, req := range reqs {
		stream.Send(req)
		time.Sleep(time.Second * 3)
	}

	res, err := stream.CloseAndRecv()
	if err != nil {
		log.Fatalf("Error receiving response: %w", err)
	}

	fmt.Println(res)
}