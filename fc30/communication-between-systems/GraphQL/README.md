# GraphQL

https://github.com/codeedu/fc2-graphql

## Run with docker-compose and access the conainer

- `docker-compose up --build -d`
- `docker exec -it fullcycle-communication-between-systems-GraphQL sh`

## Commands

- `go mod init github.com/henriqueholtz/fullcycle/fc30/communication-between-systems/GraphQL`
- [into container] `go get github.com/99designs/gqlgen@v0.13.0`
- `go run github.com/99designs/gqlgen init` (needs the file `graph/schema.graphqls`)
- `go run ./server.go`
