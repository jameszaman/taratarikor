# TARATARIKOR

This should allow you to track tickets.

## Backend

Backend service built with **.NET 9**, **Entity Framework Core**, and **PostgreSQL**.

---

### Setup

#### 1. Requirements
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for PostgreSQL)

#### 2. Start PostgreSQL
```bash
docker run --name taratarikor-pg \
  -e POSTGRES_PASSWORD=<PASSWORD> \
  -e POSTGRES_DB=<DATABASE> \
  -p 5432:5432 \
  -d postgres:16
````

#### 3. Configure Connection String

```bash
cd project/backend
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Default" "Host=<HOST>;Port=<PORT>;Database=<DATABASE>;Username=<USERNAME>;Password=<PASSWORD>"
```

#### 4. Apply Migrations

```bash
cd taratarikor/backend
dotnet ef database update
```

### Run

Use Either of the 2 commands:
```bash
cd project/backend
dotnet watch run
```

Or from root,
```bash
dotnet watch run --project backend/backend.csproj
```

API runs on [http://localhost:5000](http://localhost:5000).

