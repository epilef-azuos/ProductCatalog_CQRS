# Product Catalog API - Clean Architecture & CQRS 🚀

Uma API RESTful robusta desenvolvida em **.NET 8**, focada em demonstrar padrões avançados de arquitetura de software para sistemas corporativos e escaláveis. O projeto implementa rigorosamente a **Clean Architecture** (Arquitetura Limpa) em conjunto com o padrão **CQRS** (Command Query Responsibility Segregation).

## 🏗️ Arquitetura e Padrões de Design

O projeto foi desenhado com o objetivo de manter o baixo acoplamento e a alta coesão, isolando as regras de negócio de frameworks e detalhes de infraestrutura.

*   **Clean Architecture:** Separação física em 5 camadas lógicas (`Domain`, `Application`, `Infrastructure`, `CrossCutting` e `API`), garantindo que o Domínio não possui dependências externas.
*   **CQRS com MediatR:** Separação estrita entre operações de escrita (*Commands*) e leitura (*Queries*), utilizando o MediatR como *message bus* interno para orquestrar os fluxos.
*   **Acesso a Dados Híbrido:**
    *   **Escrita (Commands):** Utilização do **Entity Framework Core** com o padrão **Unit of Work** e **Repository** para garantir transações atômicas (ACID) e consistência de dados.
    *   **Leitura (Queries):** Utilização do **Dapper** (Micro-ORM) injetando `IDbConnection` para garantir máxima velocidade e performance nas consultas.
*   **Rich Domain Model:** Entidades (`sealed`) com comportamento encapsulado, construtores privados para o EF Core e validação de estado nativa (`DomainValidation`), evitando o antipadrão de domínio anêmico.
*   **Fail-Fast Validation:** Validação de *Commands* utilizando **FluentValidation** injetado como um *Pipeline Behavior* (`IPipelineBehavior`) no MediatR, interceptando requisições inválidas antes de atingirem os *Handlers*.
*   **Global Exception Handling:** Filtro de exceções customizado (`CustomExceptionFilter`) para formatar respostas HTTP (Ex: 400 Bad Request para erros de validação e 500 para erros de servidor).
*   **Event-Driven (Pub/Sub):** Emissão de eventos de domínio utilizando `INotification` do MediatR (ex: `ProductCreatedNotification`) para sincronização e fluxos assíncronos (notificações por email/SMS).

## 🛠️ Tecnologias e Ferramentas

*   **Linguagem/Framework:** C# 12 / .NET 8.0
*   **Base de Dados:** PostgreSQL 16
*   **ORMs:** Entity Framework Core 8 (Npgsql) & Dapper
*   **Mensageria Interna:** MediatR
*   **Validação:** FluentValidation
*   **Containerização:** Docker & Docker Compose
*   **Documentação:** Swagger (OpenAPI)

## 📁 Estrutura da Solução

```text
ProductCatalog_CQRS/
├── src/
│   ├── ProductCatalog.Domain/          # Entidades puras, Interfaces e Validações
│   ├── ProductCatalog.Application/     # Casos de Uso: Commands, Queries, Handlers, Behaviors e Notifications
│   ├── ProductCatalog.Infrastructure/  # EF Core DbContext, Repositórios, Mapeamentos e Migrations
│   ├── ProductCatalog.CrossCutting/    # Injeção de Dependências (IoC) centralizada
│   └── ProductCatalog.API/             # Controllers enxutos, Filtros Globais e Swagger
└── docker-compose.yml                  # Orquestração do PostgreSQL e da API
```

## 🚀 Como Executar o Projeto

### Pré-requisitos
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Executando com Docker (Recomendado)
A forma mais rápida de iniciar o projeto é utilizando o Docker Compose, que subirá automaticamente o PostgreSQL e a Web API configurada.

1. Clone este repositório:
   ```bash
   git clone https://github.com/epilef-azuos/ProductCatalog_CQRS.git
   ```
2. Navegue até a pasta raiz do projeto:
   ```bash
   cd ProductCatalog_CQRS
   ```
3. Execute o Docker Compose:
   ```bash
   docker-compose up --build -d
   ```
4. A API estará disponível e as migrações/seed de dados serão aplicadas automaticamente no arranque. Acesse o Swagger em:
   * **http://localhost:8080/swagger**

### Executando Localmente (Visual Studio / CLI)
Para executar a API no seu *host* local e debugar com o PostgreSQL no Docker:

1. Inicie apenas a base de dados no Docker:
   ```bash
   docker-compose up postgres_db -d
   ```
2. No Visual Studio, defina `ProductCatalog.API` como o projeto de arranque (Startup Project) com o perfil `https`.
3. Certifique-se de que a `DefaultConnection` no `appsettings.json` está apontando para `localhost`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=ProductDbCQRS;Username=postgres;Password=YourStrongPass123"
   }
   ```
4. Aplique as migrações (se necessário):
   ```powershell
   Update-Database -Project ProductCatalog.Infrastructure -StartupProject ProductCatalog.API
   ```
5. Inicie o projeto (`F5` no Visual Studio ou `dotnet run` na pasta da API).

## 📡 Endpoints Principais

A API fornece os seguintes endpoints documentados no Swagger:

*   `GET /Products` - Lista todos os produtos (Via Dapper).
*   `GET /Products/{id}` - Obtém um produto pelo ID (Via Dapper).
*   `POST /Products` - Cria um novo produto (Via EF Core + FluentValidation).
*   `PUT /Products/{id}` - Atualiza um produto existente (Via EF Core + FluentValidation).
*   `DELETE /Products/{id}` - Remove um produto (Via EF Core).

## 👨‍💻 Autor

**Felipe Assis De Souza**
* Software Developer
* [LinkedIn](#) | [GitHub](https://github.com/epilef-azuos)

---
*Desenvolvido como projeto de portfólio para aprofundamento em arquiteturas de software distribuídas e escaláveis.*
