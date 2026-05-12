# GdzieToSieJechalo

System informacji pasażerskiej dla komunikacji miejskiej.

## Struktura repozytorium

| Katalog | Opis | Technologia |
|---|---|---|
| `backend/` | REST API — rozkłady, wyszukiwanie połączeń A→B, panel zarządzania | C# .NET 10, Clean Architecture |
| `panel/` | Panel zarządzania dla dyspozytorów i adminów | TBD |
| `mobile/` | Aplikacja mobilna dla pasażerów | TBD |

## Szybki start

### Backend

```bash
cd backend
docker compose up -d
dotnet ef database update --project src/Transit.Infrastructure --startup-project src/Transit.Api
dotnet run --project src/Transit.Api
# Swagger: http://localhost:5000/swagger
```

### Testy

```bash
cd backend
dotnet test
```
