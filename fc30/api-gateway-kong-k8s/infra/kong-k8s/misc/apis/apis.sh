#!/bin/bash
kubectl create ns bets
kubectl apply -f ./apis/kratelimit.yml -n bets
kubectl apply -f ./apis/kprometheus.yml
kubectl apply -f ./apis/bets-api.yml -n bets
kubectl apply -f ./apis/king.yml -n bets