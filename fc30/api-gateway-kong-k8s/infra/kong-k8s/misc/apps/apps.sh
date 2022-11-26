#!/bin/bash
kubectl create ns bets
kubectl apply -f ./ --recursive -n bets