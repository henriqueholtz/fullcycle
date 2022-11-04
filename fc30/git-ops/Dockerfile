FROM golang:1.19 as build

WORKDIR /app
COPY . .

RUN CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -o server

FROM scratch
WORKDIR /app
COPY --from=build /app/server .
ENTRYPOINT [ "./server" ]

# docker build -t henriqueholtz/git-ops:latest .
# docker run --rm -p 8080:8080 henriqueholtz/git-ops:latest