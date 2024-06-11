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
    response_type: "token id_token code",
    // response_type "token" returns access_token
    // response_type "id_token" is useful to logout for example
    nonce,
    state
  });

  return `http://localhost:8081/realms/${realm}/protocol/openid-connect/auth?${loginUrlParams.toString()}`;
}


export function exchangeCodeForToken(code: string) {
  const tokenUrlParams = new URLSearchParams({
    client_id,
    grant_type: "authorization_code",
    code: code,
    redirect_uri: "http://localhost:3000/callback-login",
    nonce: Cookies.get("nonce") as string,
  });

  return fetch(
    `http://localhost:8081/realms/${realm}/protocol/openid-connect/token`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      body: tokenUrlParams.toString(),
    }
  )
    .then((res) => res.json())
    .then((res) => {
      return login(res.access_token, null, res.refresh_token);
    });
}

export function login(accessToken: string, idToken: string | null, refreshToken?: string, state?: string) {
  const stateCookie = Cookies.get("state");
  if (state && stateCookie !== state) {
    throw new Error("Invalid state");
  }

  let decodedAccessToken = null;
  let decodedIdToken = null;
  let decodedRefreshToken = null;
  try {
    decodedAccessToken = decodeJwt(accessToken);
    if (idToken)
      decodedIdToken = decodeJwt(idToken);

    if (refreshToken)
      decodedRefreshToken = decodeJwt(refreshToken);
  } catch (e) {
    console.error(e);
    throw new Error("Invalid token");
  }

  if (decodedAccessToken.nonce !== Cookies.get("nonce")) {
    throw new Error("Invalid nonce");
  }

  if (decodedIdToken && decodedIdToken.nonce !== Cookies.get("nonce")) {
    throw new Error("Invalid nonce");
  }
  
  if (decodedRefreshToken && decodedRefreshToken.nonce !== Cookies.get('nonce')) {
    throw new Error("Invalid nonce");
  }

  Cookies.set("access_token", accessToken);
  if (idToken)
    Cookies.set("id_token", idToken as string);

  if (decodedRefreshToken)
    Cookies.set('refresh_token', refreshToken as string)

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
  Cookies.remove("refresh_token");
  Cookies.remove("nonce");
  Cookies.remove("state");

  return `http://localhost:8081/realms/${realm}/protocol/openid-connect/logout?${logoutParams.toString()}`;
}