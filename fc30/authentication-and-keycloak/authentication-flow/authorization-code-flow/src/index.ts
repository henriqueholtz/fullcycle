import express from 'express'

const app = express()

app.get('/', (req, res) => {
    res.send('Hi there!')
})

app.get('/login', (req, res) => {
    const loginParams = new URLSearchParams({
        client_id: 'fullcycle_client',
        redirect_uri: 'http://localhost:3000/callback',
        response_type: 'code',
        scope: 'openid'
    })
    const realm = 'master'
    const url = `http://localhost:8081/realms/${realm}/protocol/openid-connect/auth?${loginParams.toString()}`
    
    res.redirect(url)
})

const port = 3000;
app.listen(port, () => {
    console.log(`Listening on port ${port}...`)
})