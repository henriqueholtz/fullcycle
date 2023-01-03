#!/bin/bash
kubectl port-forward svc/keycloak 8080:80 -n iam