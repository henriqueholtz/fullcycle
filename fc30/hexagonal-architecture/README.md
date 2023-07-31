# Hexagonal Architecture

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

### SQLite

- `touch sqlite.db`
- `sqlite3 sqlite.db`
- `create table products(id string, name string, price float, status string);`
- `.tables`
