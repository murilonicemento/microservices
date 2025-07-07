FROM alpine:latest AS builder

WORKDIR /app

COPY ../../init-db/mongodb.js ./

RUN apk add --no-cache openssl

FROM mongo:latest

COPY --from=builder /app /docker-entrypoint-initdb.d

ENV MONGO_INITDB_DATABASE=OrdersDatabase

EXPOSE 27017