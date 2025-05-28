 StudyTracker

StudyTracker is a full-stack study session tracker built with React and ASP.NET Core. It supports DevOps best practices with CI/CD, code quality tools, testing, and deployment via Docker.

 Features

* Track study sessions with subject and duration
* Relational MariaDB database
* Logging with ILogger
* Feature toggles
* Full CI/CD with GitHub Actions
* Unit testing (xUnit)
* E2E testing (Playwright)
* Mutation testing (Stryker)
* Code analysis and coverage (SonarQube)

---

Getting Started

Prerequisites

* Docker + Docker Compose
* .NET 8 SDK
* Node.js 18+

 1. Clone og start Docker services

```bash
git clone https://github.com/<your-repo>/StudyTrackers.git
cd StudyTrackers
docker compose up -d
```

Dette starter en MariaDB-database på port 3307 og indlæser migrations.

2. Start backend (.NET)

```bash
cd backend
dotnet run
```

API'et vil køre på [http://localhost:5000](http://localhost:5000)

 3. Start frontend (React)

```bash
cd frontend
npm install
npm run dev
```

Frontenden vil være tilgængelig på [http://localhost:5179](http://localhost:5179)

---

CI/CD Setup (GitHub Actions)

Automatiseret build og test pipeline:

* SonarQube analyse (quality gate "EASV way")
* Unit tests + code coverage
* Playwright E2E tests
* Stryker mutation testing
* Semantic versioning og release

Se `.github/workflows/ci.yml` for detaljer.

Kør tests lokalt

Unit tests med coverage

```bash
dotnet test tests/UnitTests/UnitTests.csproj --collect:"XPlat Code Coverage"
```

 Mutation tests (Stryker)

```bash
cd backend
npx stryker run
```

### E2E tests (Playwright)

```bash
cd frontend
npx playwright test
```

---

 Miljøvariabler og konfiguration

Feature toggles styres via `backend/FeatureToggles/toggles.json` og `appsettings.json`.

