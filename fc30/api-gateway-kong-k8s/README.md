# API Gateway With Kong and k8s (kubernetes)

https://github.com/devfullcycle/FC3-kong-automation
https://github.com/claudioed/bets-app

### Installation

- Create `kind` cluster => `infra/kong-k8s/kind/kind.sh`
- Apply `kong` with `helm` => `infra/kong-k8s/kong/kong.sh`
- Apply `prometheus` with `helm` => `infra/kong-k8s/misc/prometheus/prometheus.sh`
- Apply `keycloak` with `helm` => `infra/kong-k8s/misc/keycloak/keycloak.sh`
- Apply/Create Apps with => `infra/kong-k8s/misc/apps/apps.sh`
- Apply `plugins` => `infra/kong-k8s/misc/apis/apis.sh`
- `npm install -g @stoplight/spectral-cli`
- Apply `ArgoCD` => `infra/kong-k8s/argo/argo.sh`

### Commands

- `kubectl get pods -n kong` => Get Name
- `kubectl logs {name} proxy -f -n kong`
- `kubectl port-forward svc/keycloak 8080:80 -n iam` (Open in browser, select "Administration Console" and use login/password from `infra/kong-k8s/misc/keycloak/keycloak.sh`)

### Keycloak

- Create Ream with name `Bets`
- Create some users (with password)
- Create client with:
  - `Client authentication: true`
  - `Redirect Urls: *`

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
