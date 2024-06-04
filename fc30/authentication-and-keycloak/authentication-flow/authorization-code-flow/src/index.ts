import express from 'express'

const app = express()
const realm = 'master'
const client_id = 'fullcycle_client'
const redirect_uri = 'http://localhost:3000/callback-login'

app.get('/', (req, res) => {
    res.send("Hi there! You're in the authorization-code flow.")
})

app.get('/callback-login', async (req, res) => {
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
    
    const json = await response.json()
    console.log(json)

    res.send(json)
})


app.get('/login', (req, res) => {
    const loginParams = new URLSearchParams({
        client_id,
        redirect_uri,
        response_type: 'code',
        scope: 'openid'
    })
    const url = `http://localhost:8081/realms/${realm}/protocol/openid-connect/auth?${loginParams.toString()}`
    
    res.redirect(url)
})

const port = 3000;
app.listen(port, () => {
    console.log(`Listening on port ${port}...`)
})