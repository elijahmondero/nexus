# CLI Scaffolding & Management

The Nexus template includes automated tooling to maintain architectural consistency and accelerate feature delivery.

---

## 1. Full-Stack Scaffolder

Generates a complete vertical slice of functionality across the monorepo.

**Command:**
```bash
node cli/scaffold.js crud <Name>
```

**What it generates:**
- **Database**: Migration script with a unique timestamp.
- **Backend**: Controller, Repository, and Data Model in a dedicated Feature folder.
- **Frontend**: API hook and a specialized Page component.
- **Testing**: Backend integration test (Testcontainers), Frontend unit test (Vitest), and Full-stack E2E test (Playwright).

---

## 2. Backend CLI (.NET)

A specialized tool for managing the .NET environment and dependencies.

**Command:**
```bash
npm run nexus-backend -- <command>
```

**Key Commands:**
- `add-package <Name>`: Adds a NuGet package to the API project correctly.
- `add-feature <Name>`: Initializes a new backend feature directory.
- `db-switch <Provider>`: Interactive database provider configuration.

---

## 3. Engineering Standards
- **Vertical Slices**: Code is organized by feature, not by technical layer.
- **Automated Tests**: Every scaffolded feature includes a full suite of tests by default.
- **Immutability**: Once a migration is committed, it should never be modified.
