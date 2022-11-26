# API Gateway With Kong and k8s (kubernetes)

https://github.com/devfullcycle/FC3-kong-automation
https://github.com/claudioed/bets-app

### Installation

- Create `kind` cluster => `infra/kong-k8s/kind/kind.sh`
- Apply `prometheus` with `helm` => `infra/kong-k8s/misc/prometheus/prometheus.sh`
- Apply `kong` with `helm` => `infra/kong-k8s/kong/kong.sh`
- Apply `keycloak` with `helm` => `infra/kong-k8s/misc/keycloak/keycloak.sh`
- Apply/Create Apps with => `infra/kong-k8s/misc/apps/apps.sh`
- Apply `plugins` => `infra/kong-k8s/misc/apis/apis.sh`
- `npm install -g @stoplight/spectral-cli`
- Apply `ArgoCD` => `infra/kong-k8s/argo/argo.sh`
- Install `testkube` => `/load/infra/install.sh`
- Apply `metrics` => `infra/kong-k8s/kind/metrics-server/metrics-server.sh`

### Commands

- `kubectl get pods -n kong` => Get Name
- `kubectl logs {name} proxy -f -n kong`
- `kubectl port-forward svc/keycloak 8080:80 -n iam` (Open in browser, select "Administration Console" and use login/password from `infra/kong-k8s/misc/keycloak/keycloak.sh`)
- `kubectl get pods -A`
- `kubectl get hpa -n bets`

### Keycloak

- Get User and password in `/infra/kong-k8s/misc/keycloak/keycloak.sh`
- Create Ream with name `Bets`
- Create some users (with password)
- Create client with:
  - `ClientId=kong`
  - `Client type=OpenID Connect`
  - `Client authentication: true`
  - `Redirect Urls: *`
- Update the ClientSecret from this new Client in:
  - `/infra/kong-k8s/misc/apis/kopenid.yml`
  - `/load/create_bet_load.js`

### Helm

- `helm repo add kong https://charts.konghq.com`
- `helm repo add prometheus-community https://prometheus-community.github.io/helm-charts`
- [...]
- `helm repo update`

### ArgoCD

https://argo-cd.readthedocs.io/en/stable/getting_started/

- [Login Using CLI](https://argo-cd.readthedocs.io/en/stable/getting_started/#4-login-using-the-cli) (Get Password)
  - `kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d; echo`
  - [Windows] `kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}"; echo` + Convert from Base64 manually
- [Port Forward](https://argo-cd.readthedocs.io/en/stable/getting_started/#port-forwarding)
  - `kubectl port-forward svc/argocd-server -n argocd 8080:443`
- Login [https://localhost:8080/](https://localhost:8080/)
  - User: `admin`
  - Password: `{Get it above}`

### TestKube

https://testkube.io/download

### Run tests

- Create user at least 1 user on keycloak, set user, password and token on `/load/create_bet_load.js`
- `kubectl get servicemonitor -n kong` (to verify before run tests, if don't have anything, needs to install kong after prometheus)
  - `kubectl delete ns kong`
  - Apply `infra/kong-k8s/kong/kong.sh`
- `kubectl port-forward svc/prometheus-stack-grafana 3000:80 -n monitoring`
  - User: `admin`
  - Password: `prom-operator`
  - Import by id `7424` to import the dashboard of kong (don't forget of select `Prometheus` in the last step). Open in `Request rate`
- Apply load => `/load/infra/load.sh`

#### See logs of tests

- `kubectl get pods -n testkube`
- ` kubectl logs {id} -n testkube`
- `kubectl get pods -n bets`
- `kubectl get hpa -n bets`
  <!-- - `kubectl port-forward svc/prometheus-stack-kube-prom-prometheus 9090:80 -n monitoring` -->
