# Kafka with Go

https://github.com/codeedu/fc2-gokafka

### Commands

- `docker logs gokafka`
- `docker exec -it gokafka bash`
- `docker exec -it go-kafka-1 bash`
- `go mod init github.com/henriqueholtz/fullcycle/tree/master/fc30/kafka/go`
- `go mod tidy`
- `go run cmd/producer/main.go`
- `go run cmd/consumer/main.go`
- `go-kafka-1` => `kafka-topics --bootstrap-server=localhost:9092 --create --topic=test --partitions=3`
- `go-kafka-1` => `kafka-console-consumer --bootstrap-server=localhost:9092 --topic=test`