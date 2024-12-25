FROM nginx:alpine

RUN apk add --no-cache bash

COPY nginx.conf /etc/nginx/nginx.conf
COPY ./scripts ./scripts

CMD ["nginx", "-g", "daemon off;"]
