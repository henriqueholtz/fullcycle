# OpenTelemetry Project

https://github.com/devfullcycle/otel-example/tree/main/nodejs

To start this app run

`docker run -d -p 9411:9411 openzipkin/zipkin`
to start a zipkin container

- `npm i` => to install the packages
- `node courses.js`=> in one shell
- `node dashboard.js`=> in another shell

Then visit localhost:3001/dashboard

Now view the tracing data in zipkin : http://localhost:9411/zipkin/
