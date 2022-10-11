# Service Discovery and Consul

## Commands

#### Run docker container and access it

- `docker-compose up -d`
- `docker exec -it consul-server-{number} sh`
- `docker exec -it consul-client-{number} sh`

#### Consul (commands into the container)

- `consul agent -dev`
- `consul members`
- `curl localhost:8500/v1/catalog/nodes`

##### Servers

- `mkdir /etc/consul.d` && `mkdir /var/lib/consul` create folders
- `consul agent -server -bootstrap-expect=3 -node=consul-server-{number} -bind={ip from ifconfig} -data-dir=/var/lib/consul -config-dir=/etc/consul.d`
- `consul join {ip from ifconfig of another server}`

##### Clients

- `mkdir /var/lib/consul` create folder
- `consul agent -bind={ip from ifconfig} -data-dir=/var/lib/consul -config-dir=/etc/consul.d`
- `consul join {ip from ifconfig of another server}`
- `consul reload`
- `dig @localhost -p 8600 nginx.service.consul` (from clients/consul-01/services.json)

#### DNS

- `apk -U add bind-tools` (install dig)
- `dig @localhost -p 8600 consul01.node.consul`
- `dig @localhost -p 8600 nginx.service.consul`
- `curl localhost:8500/v1/catalog/services`
- `consul catalog nodes -service nginx`
- `consul catalog nodes -detailed` (show all IPs)
- `dig @localhost -p 8600 web.nginx.service.consul`
