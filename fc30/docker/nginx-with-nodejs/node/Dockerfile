# FROM node:alpine3.15
FROM node:slim

WORKDIR /home/node/appnode

RUN apt-get update && apt-get install -y wget

ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

COPY package*.json .

# USER node
# npm ci is equivalent of "yarn install --frozen-lockfile"
RUN npm ci
EXPOSE 3000

#CMD is executable with dockerize, because nodejs needs wait db 
# CMD ["npm", "run", "live-reload"]