# 🛍️ E-commerce Microservices

Este projeto é composto por três microserviços principais:

* **🧾 OrderService** – Gerencia pedidos
* **👤 UserService** – Gerencia usuários e autenticação
* **📦 ProductService** – Gerencia produtos

## 🚀 Tecnologias

* .NET 9 (ASP.NET Core)
* PostgreSQL, MySQL e MongoDB
* Docker & Docker Compose
* JWT para autenticação

## 📂 Endpoints Principais

### 📦 ProductService

* `GET /api/products`
* `POST /api/products`

### 👤 UserService

* `POST /api/users/register`
* `POST /api/users/login`

### 🧾 OrderService

* `POST /api/orders`
* `GET /api/orders/user/{userId}`

## 🐳 Executar com Docker

```bash
docker-compose up --build
```

Serviços disponíveis em:

* Product: `http://localhost:5001`
* User: `http://localhost:5002`
* Order: `http://localhost:5003`

## 🔐 Autenticação

* JWT gerado no login
* Enviar nas requisições protegidas:

```http
Authorization: Bearer {token}
```
