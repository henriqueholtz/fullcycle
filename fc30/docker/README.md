# Fullcycle3.0-docker

Este repositório é referente aos desafios propostos no módulo **Docker** curso **FullCycle 3.0**

## Desafio GO

[repositório da imagem GO - (henriqueholtz/go_fullcycle3.0-docker)](https://hub.docker.com/repository/docker/henriqueholtz/go_fullcycle3.0-docker)

```
docker run henriqueholtz/go_fullcycle3.0-docker
```

### Characteristics:

- :heavy_check_mark: **Docker + docker-compose**
- :heavy_check_mark: **Go**
- :heavy_check_mark: **[Docker] Multistage Building**
- :heavy_check_mark: **[Docker] imagem final com menos de 2mb**

## Desafio Nginx com Node.Js + Mysql

- Para executar, basta acessar a pasta `nginx-with-nodejs` e executar com `docker-compose up`
- Veja na imagem abaixo que caso a tabela `people` ainda não exista, ela é criada no início da aplicação NodeJs, além de inserir o registro que é listado no endpoint `/`
- Criei endpoints adicionar como `[GET] /query` e `[POST] /command/:name`

![image](https://user-images.githubusercontent.com/51380783/164166728-267d8c58-6942-4bbe-bf19-b89803c7508f.png)

##

### Characteristics:

- :heavy_check_mark: **Docker + docker-compose** (volumes, networks e mais)
- :heavy_check_mark: **NodeJs** (http server with **Express**)
- :heavy_check_mark: **Nodemon** (usado para que ocorra o "live-reload" em modificações feitas na aplicação NodeJs, sem precisar reiniciar o container manualmente)
- :heavy_check_mark: **Dockerize** (usado para que o container do NodeJs aguarde o container do Mysql estar disponível)
- :heavy_check_mark: **MySql**
- :heavy_check_mark: **Nginx** (como proxy-reverso para a aplicação http em NodeJs)
- :heavy_check_mark: **Nodemon**
