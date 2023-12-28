package events

import "errors"

var ErrorHandlerAlreadyRegistered = errors.New("Handler already registered!")

type EventDispatcher struct {
	handlers map[string][]IEventHandler
}

func NewEventDispatcher() *EventDispatcher {
	return &EventDispatcher{
		handlers: make(map[string][]IEventHandler),
	}
}

func (ed *EventDispatcher) Register(eventName string, handler IEventHandler) error {
	if _, ok := ed.handlers[eventName]; ok {
		for _, h := range ed.handlers[eventName] {
			if (h == handler) {
				return ErrorHandlerAlreadyRegistered
			}
		}
	}
	
	ed.handlers[eventName] = append(ed.handlers[eventName], handler)
	return nil
}

func (ed *EventDispatcher) Remove(eventName string, handler IEventHandler) error {
	return nil
}

func (ed *EventDispatcher) Has(eventName string, handler IEventHandler) error {
	return nil
}