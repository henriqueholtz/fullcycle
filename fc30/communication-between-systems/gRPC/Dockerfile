FROM golang:1.18-alpine

WORKDIR /go/src

RUN apk update && apk add protobuf & \
    go get google.golang.org/protobuf/cmd/protoc-gen-go & \
    go get google.golang.org/grpc & \
    go get google.golang.org/grpc/codes & \
    go get google.golang.org/grpc/reflection & \
    go get google.golang.org/grpc/status & \
    go install google.golang.org/protobuf/cmd/protoc-gen-go@latest & \
    go install google.golang.org/grpc/cmd/protoc-gen-go-grpc@latest & \
    go install google.golang.org/grpc/cmd/protoc-gen-go-grpc@v1.2 & \
    go install google.golang.org/protobuf/cmd/protoc-gen-go@v1.28
    

CMD ["tail","-f","/dev/null"]