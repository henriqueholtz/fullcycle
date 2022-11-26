import { check } from 'k6';
import http from 'k6/http';

const KONG_CLIENT = 'kong';
const KONG_SECRET = 'tyKwYz8WxCcZC9EE35KmglwVTtPQLzPZ';
const USER = 'user1';
const PASS = '123456';

export const options = {
  stages: [
    { target: 3, duration: '10s' },
    { target: 5, duration: '60s' },
  ],
  thresholds: {
    http_req_failed: [{ threshold: 'rate<0.2', abortOnFail: true }],
  },
};

function authenticateUsingKeycloak(clientId, clientSecret, username, pass) {
  const formData = {
    client_id: clientId,
    grant_type: 'password',
    username: username,
    password: pass,
    client_secret: clientSecret,
    scope: 'openid',
  };
  const headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
  const response = http.post(
    'http://keycloak.iam/realms/bets/protocol/openid-connect/token',
    formData,
    { headers }
  );
  // console.log('authenticateUsingKeycloak', response.json());
  return response.json();
}

export function setup() {
  return authenticateUsingKeycloak(KONG_CLIENT, KONG_SECRET, USER, PASS);
}

export default function (data) {
  const params = {
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${data.access_token}`, // or `Bearer ${clientAuthResp.access_token}`
    },
  };
  const payload = JSON.stringify({
    match: '1X-DC',
    email: 'joe@doe.com',
    championship: 'Uefa Champions League',
    awayTeamScore: '2',
    homeTeamScore: '3',
  });
  let response = http.post(
    'http://kong-kong-proxy.kong/api/bets',
    payload,
    params
  );
  console.log('response...', response.status);
  check(response, {
    'is status 201': (r) => r.status === 201,
  });
}
