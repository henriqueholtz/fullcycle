package main

import "fmt"

func main() {
	event := []string{"test", "test2", "test3", "test4"}
	fmt.Println(event)
	fmt.Println(event[:0])
	fmt.Println(event[1:])
	fmt.Println(event[:2])
	fmt.Println(event[2:])

	event2 := append(event[:0], event[1:]...)
	fmt.Println(event2)
}