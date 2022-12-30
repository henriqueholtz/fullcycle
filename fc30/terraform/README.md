# Terraform

https://github.com/codeedu/fc2-terraform

### Commands:

- `terraform init`
- `terraform plan`
- `terraform apply` or `terraform apply --auto-approve`
- `export TF_VAR_content="From Environment"`
- `terraform apply -var "content=From Environment line"`
- `terraform apply -var-file _terraform.tfvars`

### Notes

- Per default, terraform use `terraform.tfvars`

# AWS

### Configure AWS CLI

The settings are stored in `~/.aws/*`

- `aws configure`

  - Access Key ID
  - Secret Access Key
  - Region (ex: `us-east-1`)

- `terraform apply --auto-approve`
- `cp kubeconfig ~/.kube/config` (after apply, the `outputs.tf` will create the `kubeconfig` file)
- `kubectl get nodes -A`
- `kubectl create deploy nginx --image=nginx`
- `kubectl port-forward pod/{podName} 8181:80` (get `{podName}` from `kubectl get pods`)
- `kubectl delete deploy nginx `
- `terraform destroy`
