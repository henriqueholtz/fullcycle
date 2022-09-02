package services

import (
	"context"
	"fmt"

	"github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/gRPC/pb"
)

// type UserServiceClient interface {
// 	AddUser(ctx context.Context, in *User, opts ...grpc.CallOption) (*User, error)
// }

type UserService struct {
	pb.UnimplementedUserServiceServer
}

func NewUserService() *UserService {
	return &UserService{}
}

func (*UserService) AddUser(ctx context.Context, req *pb.User) (*pb.User, error) {

	fmt.Println(req.Name)

	return &pb.User {
		Id: "123",
		Name: req.GetName(),
		Email: req.GetEmail(),
	}, nil
}