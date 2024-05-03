package main

import (
	"fmt"

	"github.com/confluentinc/confluent-kafka-go/kafka"
)

func main() {
	fmt.Println("Starting Kafka Consumer...")
	
	configMap := &kafka.ConfigMap{
		// https://docs.confluent.io/platform/current/installation/configuration/consumer-configs.html
		"bootstrap.servers": "go-kafka-1:9092",
		"client.id": "goapp-consumer",
		"group.id": "goapp-group",
		// "auto.offset.reset": "earliest", // since always
	}

	c, err := kafka.NewConsumer(configMap)
	if err != nil {
		fmt.Println("Error at consumer", err.Error())
	}

	topics := []string{"test"}
	c.SubscribeTopics(topics, nil)

	for { 
		msg, err := c.ReadMessage(-1 /* forever */)
		if err == nil {
			fmt.Println(string(msg.Value), msg.TopicPartition)
		}
	}
}