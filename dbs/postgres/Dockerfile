FROM alpine:latest AS builder

WORKDIR /app

COPY ../../init-db/create-database-postgres.sql ./
COPY ../../init-db/postgres.sql ./

RUN apk add --no-cache openssl

FROM postgres:latest

COPY --from=builder /app /docker-entrypoint-initdb.d

ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=admin
ENV POSTGRES_DB=microservices_users

EXPOSE 5432