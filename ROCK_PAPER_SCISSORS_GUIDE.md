# Rock Paper Scissors - Implementation Guide

To build a **Rock Paper Scissors** game using the Nexus SaaS Template, you can leverage the built-in scaffolding tools to generate a true End-to-End (E2E) feature. This includes the database migration, backend API, frontend React components, and a full suite of automated tests.

## Step 1: Scaffold the Game Feature

Run the following command from the root of the repository to generate the entire vertical slice for the game:

```bash
node cli/scaffold.js crud GameMatch
```

**What this generates:**
- **Database**: A unique timestamped SQL migration script `..._CreateGameMatch.sql`.
- **Backend (API)**: `GameMatchController.cs`, `GameMatchRepository.cs`, and the `GameMatch.cs` data model.
- **Frontend (UI)**: `GameMatchPage.tsx` and a custom data-fetching hook `useGameMatch.ts`.
- **Testing**: 
  - Backend integration tests using Testcontainers (`GameMatchTests.cs`).
  - Full-stack E2E tests using Playwright (`GameMatchE2ETests.cs`).
  - Frontend component tests using Vitest (`GameMatchPage.test.tsx`).

## Step 2: Register the Feature

After running the scaffold command, you need to manually link the generated code into the application pipeline.

1. **Backend (Dependency Injection)**:
   Open `backend/src/Nexus.Api/Program.cs` and register the repository so the controller can use it:
   ```csharp
   builder.Services.AddScoped<Nexus.Api.Features.GameMatch.Repositories.GameMatchRepository>();
   ```

2. **Frontend (Routing)**:
   Open `frontend/src/App.tsx`, import the new page, and add a route inside the `<Routes>` block:
   ```tsx
   import GameMatchPage from './pages/GameMatchPage';

   // ... inside <Routes>
   <Route
     path="/gamematch"
     element={
       <ProtectedRoute>
         <GameMatchPage />
       </ProtectedRoute>
     }
   />
   ```

## Step 3: Implement Game Logic

Now that the structural boilerplate is in place, you can implement the specific rules of Rock Paper Scissors.

1. **Update the Database Model (`GameMatch.cs` and Migration Script)**:
   Add properties for `Player1Move`, `Player2Move`, and `Result`.
2. **Update the Repository (`GameMatchRepository.cs`)**:
   Adjust the SQL queries to select, insert, and update the new game state fields.
3. **Implement the Game Rules (`GameMatchController.cs`)**:
   Create a new POST endpoint (e.g., `/api/gamematch/{id}/move`) that accepts a player's move, checks if the opponent has moved, and determines the winner based on the classic rules (Rock beats Scissors, Scissors beats Paper, etc.).
4. **Build the UI (`GameMatchPage.tsx`)**:
   Replace the scaffolded data table with a game interface featuring buttons for Rock, Paper, and Scissors. Use the `useGameMatch` hook to post moves to the backend.

## Step 4: Verify with Tests

Ensure your game logic is solid by running the automatically scaffolded test suites:

- **Backend & E2E Tests (xUnit & Playwright)**:
  ```bash
  cd backend/tests/Nexus.Tests
  dotnet test
  ```
- **Frontend Unit Tests (Vitest)**:
  ```bash
  cd frontend
  npm test
  ```

By following this workflow, you demonstrate a highly efficient, test-driven approach to feature development expected of a Team Lead.
