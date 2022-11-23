#!/bin/bash
# cd W:\home\henriqueholtz\Projects\fullcycle\fc30\api-gateway-kong-k8s\infra\argo-apps\
kubectl apply -f ./players.yml -n argocd
kubectl apply -f ./matches.yml -n argocd
kubectl apply -f ./championships.yml -n argocd
kubectl apply -f ./bets.yml -n argocd