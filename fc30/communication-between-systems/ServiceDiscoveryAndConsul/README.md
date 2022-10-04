# Service Discovery and Consul

## Commands

#### Run docker container and access it

- `docker-compose up -d`
- `docker exec -it consul01 sh`

#### Consul (commands into the container)

- `consult agent -dev`
- `consul members`
- `curl localhost:8500/v1/catalog/nodes`

#### DNS

- `apk -U add bind-tools` (install dig)
- `dig @localhost -p 8600 consul01.node.consul`
