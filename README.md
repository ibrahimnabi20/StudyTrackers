# StudyTracker

StudyTracker is a full-stack DevOps exam project for tracking study sessions.

## Technologies used

- React (Vite) + Bootstrap 5
- ASP.NET Core 8
- MariaDB
- Entity Framework Core
- Serilog logging
- Feature toggles (JSON-based)
- GitHub Actions CI/CD
- Unit tests (xUnit)
- Playwright E2E tests (optional)
- Docker + Docker Compose
- SonarQube (optional)
- Mutation testing with Stryker (optional)

---

## How to run the project locally

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js (LTS)](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

### 1. Start database with Docker

```bash
docker-compose up -d
