# Transit API

Backend systemu komunikacji miejskiej (JakDojadę) — .NET 10, Clean Architecture, PostgreSQL.

## Szybki start

### 1. Wymagania

- .NET 10 SDK
- Docker + Docker Desktop (do bazy i testów integracyjnych)

### 2. Baza danych

```bash
docker compose up -d
```

Uruchamia PostgreSQL na porcie **5432** i pgAdmin na **5050**.

### 3. Migracje

```bash
export PATH="$PATH:/home/gosu/.dotnet/tools"
dotnet ef database update \
  --project src/Transit.Infrastructure \
  --startup-project src/Transit.Api
```

### 4. Uruchomienie API

```bash
dotnet run --project src/Transit.Api
```

Swagger dostępny pod: **http://localhost:5000/swagger**

### 5. Konto admina (seed demo)

- Email: `admin@transit.local`
- Hasło: `Admin123!`

Użyj `/api/v1/auth/login` → skopiuj `accessToken` → kliknij **Authorize** w Swaggerze.

---

## Testy

```bash
# Wszystkie testy
dotnet test

# Tylko domenowe (bez zależności)
dotnet test tests/Transit.Domain.UnitTests

# Tylko Application (EF InMemory)
dotnet test tests/Transit.Application.UnitTests

# Integracyjne (wymaga Dockera — uruchamia kontener Postgres)
dotnet test tests/Transit.Api.IntegrationTests
```

---

## Architektura

```
Transit.Domain          — encje, value objecty, wyjątki, stałe
Transit.Application     — CQRS (MediatR), walidacja (FluentValidation), interfejsy
Transit.Infrastructure  — EF Core + Npgsql, Identity, JWT, routing połączeń, seed
Transit.Api             — Kontrolery V1, Swagger, middleware błędów, Program.cs
```

### Kluczowe endpointy (v1)

| Metoda | Endpoint                               | Opis                                  | Auth         |
|--------|----------------------------------------|---------------------------------------|--------------|
| GET    | `/api/v1/polaczenia`                   | Wyszukiwanie połączeń A → B           | Publiczny    |
| GET    | `/api/v1/przystanki/{id}/odjazdy`      | Tablica odjazdów z przystanku         | Publiczny    |
| GET    | `/api/v1/linie/{id}/rozklad`           | Rozkład linii (typ dnia: ROB/SOB/SWI) | Publiczny    |
| POST   | `/api/v1/auth/login`                   | Logowanie (JWT)                       | Publiczny    |
| POST   | `/api/v1/auth/refresh`                 | Odświeżenie tokenu                    | Publiczny    |
| GET    | `/api/v1/przystanki`                   | Lista przystanków                     | Publiczny    |
| POST   | `/api/v1/przystanki`                   | Dodaj przystanek                      | Admin/Dysp.  |
| GET    | `/api/v1/linie`                        | Lista linii                           | Publiczny    |
| POST   | `/api/v1/linie`                        | Dodaj linię                           | Admin/Dysp.  |
| GET    | `/api/v1/kierowcy`                     | Lista kierowców                       | Admin/Dysp.  |
| POST   | `/api/v1/kierowcy`                     | Dodaj kierowcę                        | Admin/Dysp.  |
| POST   | `/api/v1/kierowcy/{id}/uprawnienia`    | Nadaj uprawnienie                     | Admin/Dysp.  |
| POST   | `/api/v1/kierowcy/{id}/badania`        | Dodaj badanie lekarskie               | Admin/Dysp.  |
| GET    | `/api/v1/pojazdy`                      | Lista pojazdów                        | Admin/Dysp.  |
| POST   | `/api/v1/pojazdy`                      | Dodaj pojazd                          | Admin/Dysp.  |
| POST   | `/api/v1/pojazdy/{id}/przeglady`       | Dodaj przegląd techniczny             | Admin/Dysp.  |
| POST   | `/api/v1/realizacje`                   | Utwórz realizację kursu               | Admin/Dysp.  |
| POST   | `/api/v1/realizacje/{id}/przypisz`     | Przypisz kierowcę + pojazd            | Admin/Dysp.  |
| POST   | `/api/v1/grafiki`                      | Dodaj wpis do grafiku pracy           | Admin/Dysp.  |
| POST   | `/api/v1/kontrole`                     | Zapisz kontrolę                       | Admin/Kont.  |
| POST   | `/api/v1/kontrole/{id}/mandaty`        | Wystaw mandat                         | Admin/Kont.  |
| POST   | `/api/v1/pasazerowie`                  | Utwórz pasażera                       | Admin/Dysp.  |
| POST   | `/api/v1/pasazerowie/{id}/bilety`      | Wystaw bilet okresowy                 | Admin/Dysp.  |

### Role

| Rola        | Opis                                     |
|-------------|------------------------------------------|
| Admin       | Pełny dostęp                             |
| Dyspozytor  | Zarządzanie operacyjne (linie, kursy)    |
| Kontroler   | Kontrole biletów i mandaty               |
| Kierowca    | (przyszłe endpointy self-service)        |
| Pasazer     | (przyszłe endpointy publiczne z kontem)  |

---

## Konfiguracja (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=transit_db;Username=transit_user;Password=transit_pass"
  },
  "Jwt": {
    "SecretKey": "CHANGE_THIS_SECRET_KEY_MIN_32_CHARACTERS_LONG!",
    "Issuer": "TransitApi",
    "Audience": "TransitClients",
    "AccessTokenExpiryMinutes": "60",
    "RefreshTokenExpiryDays": "30"
  },
  "Cors": {
    "AllowedOrigins": ["*"]
  }
}
```

> **Ważne:** Zmień `SecretKey` przed wdrożeniem na produkcję!
