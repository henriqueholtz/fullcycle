import express, { Request, Response } from 'express';
const fs = require('fs');

const startedAt = new Date();
const app = express();
const port = process.env.PORT || 3000;

app.get('/', (req: Request, res: Response) => {
  res.send(`Welcome! I'm ${process.env.name}. I'm ${process.env.age}`);
});

app.get('/healthz', (req: Request, res: Response) => {
  const durationInDays = differenceInDays(startedAt, new Date());
  res.statusCode = 200;
  res.send(`healthz! `);
});

app.get('/secrets', (req: Request, res: Response) => {
  res.send(
    `Secrets:! username: ${process.env.username}. password ${process.env.password}`
  );
});

app.get('/configmap', (req: Request, res: Response) => {
  let result;
  try {
    result = fs.readFileSync('files/readfile.txt', 'utf8');
  } catch (err) {
    console.error(err);
    result = err;
  }

  res.send(`ConfigMap: ${result}`);
});

app.listen(port, () => {
  console.log(`The application is listening on port ${port}!`);
});

function differenceInDays(fewerDate: Date, biggerDate: Date): number {
  const fewerDateutc = Date.UTC(
    fewerDate.getFullYear(),
    fewerDate.getMonth(),
    fewerDate.getDate()
  );
  const biggerDateutc = Date.UTC(
    biggerDate.getFullYear(),
    biggerDate.getMonth(),
    biggerDate.getDate()
  );
  const day: number = 1000 * 60 * 60 * 24;
  return (biggerDateutc - fewerDateutc) / day;
}
