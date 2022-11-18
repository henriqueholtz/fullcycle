# API Gateway With Kong and k8s (kubernetes)

https://github.com/devfullcycle/FC3-kong-automation

### Installation

- Create `kind` cluster => `infra/kong-k8s/kind/kind.sh`
- Apply `kong` with `helm` => `infra/kong-k8s/kong/kong.sh`
- Apply `prometheus` with `helm` => `infra/kong-k8s/misc/prometheus/prometheus.sh`
- Apply `keycloak` with `helm` => `infra/kong-k8s/misc/keycloak/keycloak.sh`
- Apply/Create Apps with => `infra/kong-k8s/misc/apps/apps.sh`
- Apply `plugins` => `infra/kong-k8s/misc/apis/apis.sh`

### Commands

- `kubectl get pods -n kong` => Get Name
- `kubectl logs {name} proxy -f -n kong`

### Helm

- `helm repo add kong https://charts.konghq.com`
- `helm repo add prometheus-community https://prometheus-community.github.io/helm-charts`
- [...]
- `helm repo update`
