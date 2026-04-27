# Getting Started

This guide provides the steps necessary to set up and run the Nexus SaaS Template.

## Development Environment

### 1. Environment Configuration
Copy the example environment file and update it with your local settings:
```bash
cp .env.example .env
```
Ensure `JWT_SECRET` is set to a secure string of at least 32 characters.

### 2. Launching Services
The template supports multiple database providers via Docker profiles. PostgreSQL is recommended for local development:
```bash
docker compose --profile postgres up -d
```

### 3. Service Access
- **Frontend**: [http://localhost:3000](http://localhost:3000)
- **API Swagger**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
- **Tracing (Zipkin)**: [http://localhost:9411](http://localhost:9411)

## Project Structure
- `backend/`: .NET 10 Web API and Integration Tests.
- `frontend/`: React application with Vite.
- `cli/`: Scaffolding engine for rapid feature implementation.
- `docs/`: Comprehensive technical documentation.
