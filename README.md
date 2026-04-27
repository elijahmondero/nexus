# Nexus SaaS Template

A production-grade monorepo starter designed for rapid development, architectural consistency, and high-quality engineering standards.

## 🚀 Architecture
- **Backend**: .NET 10 Web API, Dapper, DbUp Migrations.
- **Frontend**: React 19, Vite, Material UI v6.
- **Testing**: xUnit & Testcontainers (Integration), Playwright (E2E), Vitest (Frontend Unit).
- **Observability**: OpenTelemetry & Zipkin.
- **Infrastructure**: Docker Compose with multi-database support (Postgres, SQL Server, MySQL).

## 🛠️ Getting Started

### Prerequisites
- Docker Desktop
- .NET 10 SDK
- Node.js v20+

### Setup
1. Clone the repository.
2. `cp .env.example .env`
3. Configure your secrets in `.env`.
4. Start the environment:
   ```bash
   docker compose --profile postgres up -d
   ```

## 📁 Documentation
Detailed guides are available in the `docs/` directory:
- [Getting Started Guide](./docs/getting-started.md)
- [CLI Reference](./docs/cli.md)
- [Testing Standards](./docs/testing-guide.md)

## ⚡ CLI Scaffolding
Accelerate your development with built-in code generation tools:
- **Full-Stack Feature**: `node cli/scaffold.js crud <Name>`
- **Backend Management**: `npm run nexus-backend`
