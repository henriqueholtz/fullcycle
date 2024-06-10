import Cookies from "js-cookie";
import { decodeJwt } from "jose";

const realm = 'master'
const client_id = 'fullcycle_client'
const redirect_uri = 'http://localhost:3000/callback-login'

export function makeLoginUrl() {
  const nonce = Math.random().toString(36);
  const state = Math.random().toString(36);

  // In production should be HTTPS
  Cookies.set("nonce", nonce);
  Cookies.set("state", state);

  const loginUrlParams = new URLSearchParams({
    client_id,
    redirect_uri,
    response_type: "token id_token",
    // response_type "token" returns access_token
    nonce,
    state
  });

  return `http://localhost:8081/realms/${realm}/protocol/openid-connect/auth?${loginUrlParams.toString()}`;
}

export function login(accessToken: string, idToken: string, state: string) {
  const stateCookie = Cookies.get("state");
  if (stateCookie !== state) {
    throw new Error("Invalid state");
  }

  let decodedAccessToken = null;
  let decodedIdToken = null;
  try {
    decodedAccessToken = decodeJwt(accessToken);
    decodedIdToken = decodeJwt(idToken);
  } catch (e) {
    throw new Error("Invalid token");
  }

  if (decodedAccessToken.nonce !== Cookies.get("nonce")) {
    throw new Error("Invalid nonce");
  }

  if (decodedIdToken.nonce !== Cookies.get("nonce")) {
    throw new Error("Invalid nonce");
  }

  Cookies.set("access_token", accessToken);
  Cookies.set("id_token", idToken);

  return decodedAccessToken;
}

export function getAuth() {
  const token = Cookies.get("access_token");

  if (!token) {
    return null;
  }

  try {
    return decodeJwt(token);
  } catch (e) {
    console.error(e);
    return null;
  }
}

export function makeLogoutUrl() {
  if (!Cookies.get("id_token")) {
    return false;
  }
  const logoutParams = new URLSearchParams({
    id_token_hint: Cookies.get("id_token") as string,
    post_logout_redirect_uri: redirect_uri,
  });

  Cookies.remove("access_token");
  Cookies.remove("id_token");
  Cookies.remove("nonce");
  Cookies.remove("state");

  return `http://localhost:8081/realms/${realm}/protocol/openid-connect/logout?${logoutParams.toString()}`;
}