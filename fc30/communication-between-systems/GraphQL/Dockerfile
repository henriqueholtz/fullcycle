FROM golang:1.15-alpine

# É importante criar um novo usuário para que você consiga alterar os arquivos via VS Code remoto
RUN apk update && apk add alpine-sdk && \
    adduser -u 1000 -h /go/src/mycustomuser -D mycustomuser

USER mycustomuser

WORKDIR /go/src/mycustomuser/graphql

CMD ["tail","-f","/dev/null"]