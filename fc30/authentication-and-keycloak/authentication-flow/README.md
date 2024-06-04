# Authentication Flows

## Prerequisites

- Create a client on keycloak:
    - `Client ID`: `fullcycle_client`
    - `Client authentication` and `Authorization` should be turned off
    - `Authentication flow`: Check only `Standard flow` and `Direct access grants`
    - `Root URL`: `http://localhost:3000`
    - `Valid redirect URIs` and `Web origins`: `/*`

### Commands

- `docker compose exec app bash`