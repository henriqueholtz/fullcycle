#!/bin/bash

echo "Creating kind cluster"
cd infra/kong-k8s/kind
sh kind.sh
sleep 10

echo "Applying Prometheus"
cd ../misc/prometheus
sh prometheus.sh
sleep 30

# wait for pods are running
echo "Applying keycloak"
cd ../misc/keycloak
sh keycloak.sh # wait for pods are running
sleep 40

echo ""
echo "Initializing port-forward to keycloak."
echo "Access http://localhost:8080/ to configure it (see on README)"
echo "Needs:"
echo "- Create Realm 'bets'."
echo "- Create User 'user1' and set up your password."
echo "- Create Client 'kong'."
echo "- Copy the SecretKey from new Client and put it into:"
echo "     - /infra/kong-k8s/misc/apis/kopenid.yml"
echo "     - /load/create_bet_load.js"
echo "After that run 'sh finish-create-and-run-testkube.sh'"
sh ../../port-forward-keycloak.sh # & PIDKEY=$!