#!/bin/bash
kind create cluster --name kong-fc --config clusterconfig.yml
kubectl cluster-info --context kind-kong-fc