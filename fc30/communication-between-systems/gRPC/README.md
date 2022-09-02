# gRPC

- https://github.com/codeedu/fc2-grpc

### Run

- `docker-compose up --build -d`

### Server

- `docker exec -it fullcycle-communication-between-systems-gRPC sh`
- into container: `go run cmd/server/server.go`

### Client

- `docker exec -it fullcycle-communication-between-systems-gRPC sh`
- into container: `go run cmd/client/client.go`

### Commands

- `go mod init github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/gRPC`
- [deprecated] `protoc --proto_path=proto proto/*.proto --go_out=pb` (generate `*.pb.go` files)
- `protoc --proto_path=proto/ proto/*.proto --plugin=$(go env GOPATH)/bin/protoc-gen-go-grpc --go-grpc_out=. --go_out=.` (generate `*_grpc.pb.go` files)
- into container: `go run cmd/server/server.go`
- [Require `ktr0731/evans` installed] `evans -r repl --host localhost --port 50051`
  - `service UserService` (to set the service to `UserService`)
- `call AddUser`
  -> Put the fields (As id, name, email)

- `go run cmd/client/client.go`
- `protoc --proto_path=proto proto/*.proto --go_out=pb --go-grpc_out=pb`
