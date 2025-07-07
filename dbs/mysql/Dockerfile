FROM alpine:latest AS builder

WORKDIR /app

COPY ../../init-db/mysql.sql /app

RUN apk add --no-cache openssl

FROM mysql:9.3.0

COPY --from=builder /app /docker-entrypoint-initdb.d

ENV MYSQL_ROOT_PASSWORD=admin

EXPOSE 3306