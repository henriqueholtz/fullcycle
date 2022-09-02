package main

import (
	"context"
	"fmt"
	// "io"
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
	AddUser(client)

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