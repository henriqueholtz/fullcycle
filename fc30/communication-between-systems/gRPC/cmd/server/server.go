package main

import (
	"log"
	"net"
    "fmt"

	"github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/gRPC/pb"
	"github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/gRPC/services"
	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"
)

func main() {
	
	lis, err := net.Listen("tcp", "localhost:50051")
	if err != nil {
		log.Fatalf("Couldn't connect: %v", err)
	} else {
		fmt.Println("connected successfully")
	}
	
	grpcServer := grpc.NewServer()
	pb.RegisterUserServiceServer(grpcServer, services.NewUserService())
	// pb.RegisterUserServiceServer(grpcServer, &services.UserService{})
	reflection.Register(grpcServer)

	if err := grpcServer.Serve(lis); err != nil {
		log.Fatalf("Couldn't serve: %v", err)
	} else {
		fmt.Println("grpcServer.Server successfully")
	}
}