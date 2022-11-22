#!/bin/bash
# cd W:\home\henriqueholtz\Projects\fullcycle\fc30\api-gateway-kong-k8s\infra\kong-k8s\misc
# maybe needs to comment "konghq.com/plugins" on "./apis/bets-api.yml"
kubectl create ns bets
kubectl apply -f ./apis/kratelimit.yml -n bets
kubectl apply -f ./apis/kprometheus.yml
kubectl apply -f ./apis/bets-api.yml -n bets
kubectl apply -f ./apis/king.yml -n bets

# Needs keycloak configurated (see in README)
kubectl apply -f ./apis/kopenid.yml -n bets

# uncomment "konghq.com/plugins" with the value "oidc-bets" on "./apis/bets-api.yml"
kubectl apply -f ./apis/bets-api.yml -n bets