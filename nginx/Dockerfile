FROM nginx:alpine

RUN apk add --no-cache bash

COPY ./ /etc/nginx/
COPY ./scripts ./scripts

CMD ["nginx", "-g", "daemon off;"]
