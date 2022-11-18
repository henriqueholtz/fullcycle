#!/bin/bash
kubectl create ns bets
kubectl apply -f ./apps --recursive -n bets