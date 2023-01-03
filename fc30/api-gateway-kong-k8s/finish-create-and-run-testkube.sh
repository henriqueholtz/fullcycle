#!/bin/bash
# wait for pods are running
echo "Applying Kong"
cd ../../kong
sh kong.sh
sleep 15

# wait for pods are running
echo "Applying Apps"
cd ../misc/apps
sh apps.sh
sleep 15

# wait for pods are running
echo "Applying Apis"
cd ../apis
sh apis.sh
sleep 15

# wait for pods are running
echo "Applying metrics-server"
cd ../../kind/metrics-server
sh metrics-server.sh
sleep 15

# wait for pods are running
cd ../../../../load/infra
sh install.sh
sleep 30

echo "Initializing tests + port-forward to grafana."
echo "Access the grafana: http://localhost:3000"
sh ../../port-forward-prometheus-grafana.sh & PIDPROM=$!
sh load.sh & PIDLOAD=$!
wait $PIDPROM
wait $PIDLOAD
