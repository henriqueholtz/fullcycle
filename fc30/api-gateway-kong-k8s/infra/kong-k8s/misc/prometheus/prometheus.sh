#!/bin/bash
kubectl create ns monitoring
helm install prometheus-stack prometheus-community/kube-prometheus-stack -f prometheus.yml --namespace monitoring