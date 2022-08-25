# gRPC

- https://github.com/codeedu/fc2-grpc

### Commands

- `go mod init github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/gRPC`
- [deprecated] `protoc --proto_path=proto proto/*.proto --go_out=pb` (generate `*.pb.go` files)
- `protoc --proto_path=proto/ proto/*.proto --plugin=$(go env GOPATH)/bin/protoc-gen-go-grpc --go-grpc_out=. --go_out=.` (generate `*_grpc.pb.go` files)
