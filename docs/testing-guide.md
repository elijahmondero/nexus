# Testing Standards & Execution

We employ a multi-layered testing strategy to ensure the stability and correctness of the platform.

---

## 1. Backend Integration Tests (.NET)
We use **Testcontainers** to verify the API against real database instances.

- **Command**: `dotnet test backend/Nexus.slnx --filter "Category!=E2E"`
- **Tooling**: xUnit, FluentAssertions, Testcontainers.PostgreSql.

---

## 2. End-to-End Tests (Browser)
We use **Playwright (.NET)** to verify user journeys across the full stack.

- **Command**: `dotnet test backend/Nexus.slnx --filter "FullyQualifiedName~E2E"`
- **Setup**: Ensure the frontend is running (`docker compose up`) before executing E2E tests.

---

## 3. Frontend Unit Tests (React)
We use **Vitest** for component-level verification.

- **Command**: `cd frontend && npm test`
- **Tooling**: Vitest, React Testing Library.

---

## 4. Quality Gates
- **Naming**: Tests must follow the `Method_Scenario_ExpectedResult` pattern.
- **Isolation**: Each integration test must run against an isolated database state.
- **Coverage**: All new features scaffolded via the CLI are required to maintain their generated tests.
