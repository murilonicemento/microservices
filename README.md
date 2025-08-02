# 🧱 Microservices Architecture with .NET, Docker, RabbitMQ & Azure

Este projeto é uma aplicação baseada em arquitetura de microsserviços, utilizando .NET, Docker, RabbitMQ, PostgreSQL, Kubernetes (AKS) e integração com serviços da Azure como o Key Vault.

## 📦 Estrutura dos Microsserviços

- **Microservices.ApiGateway**: API Gateway para rotear requisições aos microsserviços.
- **Microservices.OrderService**: Serviço responsável pela lógica de pedidos.
- **Microservices.ProductService**: Serviço responsável pelos produtos.
- **Microservices.UserService**: Serviço responsável pela autenticação e gerenciamento de usuários.

## ⚙️ Componentes de Infraestrutura

- **dbs/**: Scripts e recursos para criação e build do ambiente de banco de dados via Docker.
- **init-db/**: Scripts de inicialização do banco de dados PostgreSQL.
- **aks/**: Manifests e arquivos secretos para deploy em Kubernetes (AKS).
- **docker-compose.build.yaml**: Compose usado para build e deploy dos serviços.
- **docker-compose.development.yaml**: Compose para ambiente de desenvolvimento.

## ☁️ Integrações

- **RabbitMQ**: Utilizado como broker para comunicação assíncrona entre serviços.
- **Azure Key Vault**: Gerenciamento de segredos sensíveis (como strings de conexão).
- **Azure Kubernetes Service (AKS)**: Orquestração e deploy dos microsserviços.

## 🚀 Como executar localmente

### Pré-requisitos

- [.NET SDK 7+](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [PostgreSQL](https://www.postgresql.org/)
- [RabbitMQ](https://www.rabbitmq.com/)
- (Opcional) [kubectl](https://kubernetes.io/docs/tasks/tools/) e [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)

### Passos

1. Clone o repositório:

```bash
 git clone <url-do-repo>
 cd <nome-do-projeto>
```

2. Configure seu `.env` a partir do `.env.example`.

3. Suba os containers com:

```bash
 docker-compose -f docker-compose.development.yaml up --build
```

4. Acesse os serviços nas portas definidas (ver `docker-compose` para detalhes).

## 🧪 Testes

Testes unitários são implementados com **xUnit**. Você pode executá-los via terminal com:

```bash
dotnet test
```

## 📁 Outros arquivos

- `.dockerignore` / `.gitignore`: Arquivos de exclusão de build e git.
- `LICENSE`: Licença do projeto.
- `Microservices.sln`: Solução principal do projeto no Visual Studio.

## 📌 Roadmap futuro (ideias)

- Monitoramento com Prometheus + Grafana
- Logging estruturado com Serilog + ILogger
- Integração com Redis e ElasticSearch
- CI/CD com Azure Pipelines
- Documentação com Swagger

## 📄 Licença

Distribuído sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
