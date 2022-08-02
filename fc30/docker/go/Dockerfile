### Builder
FROM golang:alpine3.15 as builder

WORKDIR /go/builder

COPY main.go /go/builder/

RUN go build /go/builder/main.go


### Execute from builder
FROM scratch

WORKDIR /go/src
COPY --from=builder /go/builder/main .
CMD ["/go/src/main"]