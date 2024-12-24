FROM node:18-alpine

RUN apk add --no-cache openssl

WORKDIR /app

COPY package*.json ./

RUN npm install react-loading && npm install

COPY . .

RUN mkdir -p ~/.aspnet/https

RUN openssl req -x509 -newkey rsa:4096 -keyout ~/.aspnet/https/billingapplication.client.key -out ~/.aspnet/https/billingapplication.client.pem -days 365 -nodes \
    -subj "/C=US/ST=Some-State/L=Some-City/O=MyCompany/OU=MyOrg/CN=localhost"

EXPOSE 5173

CMD ["npm", "run", "dev"]
