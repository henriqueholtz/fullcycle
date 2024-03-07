# EDA - Event Driven Architecture

Note: Some files came from [architecture-microservices-based](../architecture-microservices-based/ms/walletcore/)

- **Event notification**: Only a specific part of the data
- **Event Carried State Transfer**: The complete (or almost) data
- **Event sourcing**: Based as a timeline. You might run a replay of all events. (ex: bank account balance)
- **Event colaboration**: https://martinfowler.com/eaaDev/EventCollaboration.html

https://github.com/devfullcycle/fc-eda/blob/main/pkg/events/event_dispatcher_test.go

## Commands

- `go test ./...`
- `go mod init github.com/henriqueholtz/fullcycle/fc30/event-driven-architecture/utils`
- `go mod tidy`
- `go run cmd/walletcore/main.go`
- `docker-compose exec mysql bash`
- `mysql -uroot -p wallet` (password from docker-compose.yml: `root` )
- Scripts SQL to create the tables are on `./internal/database/transaction_db_test.go`
