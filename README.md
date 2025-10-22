# TestBank

## Run with Docker Compose

Prerequisites:
- Docker Desktop

### Quick start

1) From the repository root, build and start the stack in the background:

```bash
docker compose up --build
```

2) Wait until both services are healthy:

```bash
docker compose ps
```

3) Open Swagger UI to explore the API endpoints:
- http://localhost:8080/swagger/index.html

4) Optional health check:

```bash
curl http://localhost:8080/healthz
```

### Useful commands

- View container logs (API):

```bash
docker compose logs -f api
```

- Stop and remove the stack:

```bash
docker compose down
```

- Stop, remove and purge data volume:

```bash
docker compose down -v
```

### Tech Stack
- Database: PostgresSQL
- ORM: EF Core
- Framework: ASP.NET Core 9
For unit tests: xunit, NSubstitute, FluentAssertions
For integration tests: xunit, NSubstitute, FluentAssertions + Testcontainers.PostgreSql

### Solution design and patterns
DDD + CQRS (without MediatR)
Repository pattern (without generic repository) + Unit of Work

### Notes
For resolving race conditions in case, when some parallel operation happenning on AccountBalance was choosen Optimistic Concurency throught Version column on PostgresSQL side (xmin) with retry logic with reprocessing of business logic.