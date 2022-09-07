package main

import (
	"context"
	"fmt"
	"io"
	"log"
	// "time"

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
	AddUserVerbose(client)

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