# GitOps

https://github.com/devfullcycle/gitopsfc

## Commands

Creating cluster with kind:

- `kind create cluster --name=gitopsfc`
- `kubectl cluster-info --context kind-gitopsfc`

Apply changes (after push the commit with new tag generated by CD on Github Actions)

- `kubectl apply -f k8s`

#### Kind commands

- `kind get clusters`

#### K8S commands

- `kubectl get svc`
- `kubectl get pods`
- `kubectl get ns`
- `kubectl get deploy`

#### ArgoCD

https://argo-cd.readthedocs.io/en/stable/getting_started

Install:

- `kubectl create namespace argocd`
- `kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml`

Get the password:

- Linux: `kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}" | base64 -d; echo`
- Windows `kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}"; echo` (needs to convert from base64 manually)

Run with Port Forwarding:

- `kubectl port-forward svc/argocd-server -n argocd 8080:443`

Access https://localhost:8080/

- Username: `admin`
- Password: `{Get the password above}`
