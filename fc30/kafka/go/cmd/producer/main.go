package main

import (
	"fmt"
	"log"

	"github.com/confluentinc/confluent-kafka-go/kafka"
)

func main() {
	fmt.Println("Starting Kafka Producer...")
	producer := NewKafkaProducer()
	PublishMessage("message 1", "test", producer, nil)
	producer.Flush(1 * 1000)
	fmt.Println("Ending Kafka Producer...")
}

func NewKafkaProducer() *kafka.Producer {
	configMap := &kafka.ConfigMap{
		"bootstrap.servers": "go-kafka-1:9092",
	}
	p, err := kafka.NewProducer(configMap)

	if err != nil {
		log.Println(err.Error())
	}
	return p
}

func PublishMessage(msg string, topic string, producer *kafka.Producer, key []byte) error {
	kafkaMsg := &kafka.Message{
		Value: []byte(msg),
		TopicPartition: kafka.TopicPartition{Topic: &topic, Partition: kafka.PartitionAny},
		Key: key,
	}

	err := producer.Produce(kafkaMsg, nil)
	if err != nil {
		return err
	}

	return nil
}