# gRPC

- https://github.com/codeedu/fc2-grpc

### Commands

- `go mod init github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/gRPC`
- `protoc --proto_path=proto proto/*.proto --go_out=pb` (generate `*.pb.go` files)
- `protoc --proto_path=proto proto/*.proto --go_out=pb --go-grpc_out=pb` (generate `*_grpc.pb.go` files)
