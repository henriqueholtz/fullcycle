import express from 'express'
import session from "express-session";
import jwt from "jsonwebtoken";
import crypto from 'crypto'

const realm = 'master'
const client_id = 'fullcycle_client'
const redirect_uri = 'http://localhost:3000/callback-login'

const app = express()

const memoryStore = new session.MemoryStore();

app.use(
  session({
    secret: "my-secret",
    resave: false,
    saveUninitialized: false,
    store: memoryStore,
    //expires
  })
);

app.get('/', (req, res) => {
    res.send("Hi there! You're in the authorization-code flow.")
})

app.get('/callback-login', async (req, res) => {
    
    //@ts-expect-error - type mismatch
    if (req.session.user) {
        return res.redirect("/admin");
    }

    //@ts-expect-error - type mismatch
    if(req.query.state !== req.session.state) {
      //poderia redirecionar para o login em vez de mostrar o erro
      return res.status(401).json({ message: "Unauthenticated" });
    }
  
    console.log(req.query)

    const bodyParams = new URLSearchParams({
        client_id,
        grant_type: 'authorization_code',
        code: req.query.code as string,
        redirect_uri
    })
    const host = 'localhost' //'keycloak' // host.docker.internal
    const url = `http://${host}:8081/realms/${realm}/protocol/openid-connect/token`

    const response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: bodyParams.toString()
    })
    
    const jsonResult = await response.json()
    console.log(jsonResult)
    const payloadAccessToken = jwt.decode(jsonResult.access_token) as any;
    const payloadRefreshToken = jwt.decode(jsonResult.refresh_token) as any;
    const payloadIdToken = jwt.decode(jsonResult.id_token) as any;
  
    if (
      //@ts-expect-error - type mismatch
      payloadAccessToken!.nonce !== req.session.nonce ||
      //@ts-expect-error - type mismatch
      payloadRefreshToken.nonce !== req.session.nonce ||
      //@ts-expect-error - type mismatch
      payloadIdToken.nonce !== req.session.nonce
    ) {
      return res.status(401).json({ message: "Unauthenticated" });
    }
  
    console.log(payloadAccessToken);
    //@ts-expect-error - type mismatch
    req.session.user = payloadAccessToken;
    //@ts-expect-error - type mismatch
    req.session.access_token = jsonResult.access_token;
    //@ts-expect-error - type mismatch
    req.session.id_token = jsonResult.id_token;
    req.session.save();

    res.send(jsonResult)
})


app.get('/login', (req, res) => {

    const nonce = crypto.randomBytes(16).toString('base64')

    //@ts-expect-error - type mismatch
    req.session.nonce = nonce;
    req.session.save();

    const loginParams = new URLSearchParams({
        client_id,
        redirect_uri,
        response_type: 'code',
        scope: 'openid',
        nonce
    })
    const url = `http://localhost:8081/realms/${realm}/protocol/openid-connect/auth?${loginParams.toString()}`
    console.log(url)
    res.redirect(url)
})

const port = 3000;
app.listen(port, () => {
    console.log(`Listening on port ${port}...`)
})