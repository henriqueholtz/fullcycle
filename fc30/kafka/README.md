# Kafka

https://github.com/codeedu/fc2-kafka

[Control Center](http://localhost:9021/)

### Commands

- `docker-compose ps`
- `docker exec -it kafka-kafka-1 bash`
- `kafka-topics --bootstrap-server=localhost:9092 --create --topic=test --partitions=3`
- `kafka-topics --bootstrap-server=localhost:9092 --list`
- `kafka-topics --bootstrap-server=localhost:9092 --topic=test --describe`
- `kafka-console-consumer --bootstrap-server=localhost:9092 --topic=test --group=123`
- `kafka-console-consumer --bootstrap-server=localhost:9092 --topic=test --from-beginning`
- `kafka-console-producer --bootstrap-server=localhost:9092 --topic=test`
- `kafka-consumer-groups --bootstrap-server=localhost:9092 --group=123 --describe`