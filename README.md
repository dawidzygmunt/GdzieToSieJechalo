# GdzieToSieJechalo

System informacji pasażerskiej dla komunikacji miejskiej.

## Struktura repozytorium

| Katalog | Opis | Technologia |
|---|---|---|
| `backend/` | REST API — rozkłady, wyszukiwanie połączeń A→B, panel zarządzania | C# .NET 10, Clean Architecture |
| `panel/` | Panel zarządzania dla dyspozytorów i adminów | React + TypeScript + Vite |
| `mobile/` | Aplikacja mobilna dla pasażerów | TBD |

## Domyślne konto administratora

Po pierwszym uruchomieniu systemu automatycznie tworzone jest konto admina:

- **Email:** `admin@transit.local`
- **Hasło:** `Admin123!`
- **Rola:** Admin (pełne uprawnienia)

📖 Więcej informacji: [DOMYSLNE_KONTO.md](./DOMYSLNE_KONTO.md)

## Szybki start

### Backend

```bash
cd backend
docker compose up -d
dotnet ef database update --project src/Transit.Infrastructure --startup-project src/Transit.Api
dotnet run --project src/Transit.Api
# Swagger: http://localhost:5210/swagger
# API: http://localhost:5210/api/v1
```

### Panel (Frontend)

```bash
cd panel
npm install
npm run dev
# Panel: http://localhost:3000
```

Zaloguj się używając domyślnego konta: `admin@transit.local` / `Admin123!`

### Testy

**Backend:**
```bash
cd backend
dotnet test
```

**Frontend:**
```bash
cd panel
npm test              # Uruchom wszystkie testy
npm run test:ui       # Testy z interfejsem UI
npm run test:coverage # Testy z coverage
```

📖 Dokumentacja testów: [panel/TESTING.md](./panel/TESTING.md)
