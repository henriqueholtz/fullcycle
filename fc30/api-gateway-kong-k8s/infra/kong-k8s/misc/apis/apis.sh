#!/bin/bash
# cd W:\home\henriqueholtz\Projects\fullcycle\fc30\api-gateway-kong-k8s\infra\kong-k8s\misc
# maybe needs to comment "konghq.com/plugins" on "./apis/bets-api.yml"
kubectl create ns bets
kubectl apply -f ./kratelimit.yml -n bets
kubectl apply -f ./kprometheus.yml
kubectl apply -f ./bets-api.yml -n bets
kubectl apply -f ./king.yml -n bets

# Needs keycloak configurated (see in README)
kubectl apply -f ./kopenid.yml -n bets

# uncomment "konghq.com/plugins" with the value "oidc-bets" on "./bets-api.yml"
kubectl apply -f ./bets-api.yml -n bets

# Apply all
#kubectl apply -f ./ --recursive -n bets