package main

import (
	"fmt"
	"log"

	"github.com/confluentinc/confluent-kafka-go/kafka"
)

func main() {
	fmt.Println("Starting Kafka Producer...")

	deliveryChannel := make(chan kafka.Event)
	producer := NewKafkaProducer()
	PublishMessage("message 1", "test", producer, nil/*[]byte("my-key")*/, deliveryChannel)

	/* sync
	e := <- deliveryChannel
	msg := e.(*kafka.Message)
	if msg.TopicPartition.Error != nil {
		fmt.Println("Error while sending a kafka message!")
	} else {
		fmt.Println("A kafka message was successfuly sent!", msg.TopicPartition)
	}*/
	
	go DeliveryReport(deliveryChannel) // async
	producer.Flush(1000)
	
	fmt.Println("Ending Kafka Producer...")
}

func NewKafkaProducer() *kafka.Producer {
	configMap := &kafka.ConfigMap{
		// https://docs.confluent.io/platform/current/installation/configuration/producer-configs.html
		"bootstrap.servers": "go-kafka-1:9092",
		"delivery.timeout.ms": "1000",
		"acks": "all",
		"enable.idempotence": "false",
	}
	p, err := kafka.NewProducer(configMap)

	if err != nil {
		log.Println(err.Error())
	}
	return p
}

func PublishMessage(msg string, topic string, producer *kafka.Producer, key []byte, deliveryChannel chan kafka.Event) error {
	kafkaMsg := &kafka.Message{
		Value: []byte(msg),
		TopicPartition: kafka.TopicPartition{Topic: &topic, Partition: kafka.PartitionAny},
		Key: key,
	}

	err := producer.Produce(kafkaMsg, deliveryChannel)
	if err != nil {
		return err
	}

	return nil
}

func DeliveryReport(deliveryChannel chan kafka.Event) {
	for e := range deliveryChannel {
		switch ev := e.(type) {
		case *kafka.Message:
			if ev.TopicPartition.Error != nil {
				fmt.Println("Error while sending a kafka message!")
			} else {
				fmt.Println("A kafka message was successfuly sent!", ev.TopicPartition)
			}
		}
	}
}