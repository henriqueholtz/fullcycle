#!/bin/bash
kubectl port-forward svc/prometheus-stack-grafana 3000:80 -n monitoring 