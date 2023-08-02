# Hexagonal Architecture (Ports and adapters)

https://github.com/codeedu/fc2-arquitetura-hexagonal

## Commands

- `docker exec -it appproduct bash`
- `go mod init github.com/henriqueholtz/fullcycle/fc30/hexagonal-architecture`
- `go get github.com/stretchr/testify/require@v1.7.0`
- `go test ./...`
- `go get github.com/asaskevich/govalidator@v0.0.0-20210307081110-f21760c49a8d`
- `go get github.com/satori/go.uuid@v1.2.0`
- `mockgen -destination=application/mocks/application.go -source=application/product.go application`
- `go get github.com/golang/mock/gomock`
- `go get github.com/mattn/go-sqlite3@v1.14.7`
- `go get -u github.com/spf13/cobra@v1.1.3`
- `go mod tidy` (remove unused packages and add the missing packages)
- `go get github.com/codegangsta/negroni@v1.0.0`
- `go get github.com/gorilla/mux@v1.8.0`

### SQLite

- `touch sqlite.db`
- `sqlite3 sqlite.db`
- `create table products(id string, name string, price float, status string);`
- `.tables`
- `SELECT * FROM products;`

### Cobra CLI

- `cobra-cli init` (old `cobra init --pkg-name=github.com/henriqueholtz/fullcycle/fc30/hexagonal-architecture`)
- `cobra-cli add cli` ("cli" is the name of the command)
- `go run main.go cli`
- `go run main.go cli -a="create" -name="Product CLI" -p=25.0`
- `go run main.go cli -a="get" -i="a532d919-420b-444b-aa79-7fae4f1ec577"`
- `cobra-cli add http` ("http" is the name of the command)
- `go run main.go http`
- `curl http://localhost:9000/product/invalid-productId`
- `curl http://localhost:9000/product/a532d919-420b-444b-aa79-7fae4f1ec577`
