# Podsumowanie projektu GdzieToSieJechalo

**Data:** 2026-05-12  
**Repo:** [github.com/dawidzygmunt/GdzieToSieJechalo](https://github.com/dawidzygmunt/GdzieToSieJechalo)  
**Branch:** `main` (8 commitów)

---

## 1. Co to jest

System informacji pasażerskiej dla komunikacji miejskiej — odpowiednik aplikacji „JakDojadę". Projekt zorganizowany jako **monorepo** z wydzielonymi miejscami na trzy aplikacje.

---

## 2. Struktura monorepo

```
GdzieToSieJechalo/
├── backend/        ← REST API — gotowe
├── panel/          ← Panel zarządzania — placeholder (TBD)
├── mobile/         ← Aplikacja mobilna — placeholder (TBD)
├── .gitignore
└── README.md
```

---

## 3. Backend — stan na dziś

### Stack technologiczny

| Warstwa | Technologia |
|---|---|
| Framework | .NET 10 |
| Baza danych | PostgreSQL 16 (Npgsql + EF Core 10) |
| Architektura | Clean Architecture (4 warstwy) |
| CQRS | MediatR 12 + 3 pipeline behaviors |
| Walidacja | FluentValidation 11 |
| Mapowanie | Mapster 10 |
| Auth | ASP.NET Identity + JWT (access + refresh token) |
| Dokumentacja API | Swashbuckle (Swagger) + wersjonowanie URL (`/api/v1/`) |
| Logowanie | Serilog (konsola) |
| Testy jednostkowe | xUnit + FluentAssertions + NSubstitute + EF InMemory |
| Testy integracyjne | xUnit + WebApplicationFactory + Testcontainers (Postgres w Dockerze) |
| Migracje | EF Core Migrations (2 migracje) |
| Konteneryzacja | Docker Compose (Postgres + pgAdmin) |

### Metryki kodu

| Co | Liczba |
|---|---|
| Pliki `.cs` (bez wygenerowanych) | 120 |
| Linie kodu (bez migracji) | ~4 000 |
| Pakiety NuGet (centralnie w Directory.Packages.props) | 29 |
| CQRS handlers (Commands + Queries) | 25 |
| Testy łącznie | 28 |
| Endpointy API | 22 |
| Tabele w bazie | 25 + tabele ASP.NET Identity |

---

## 4. Architektura — warstwy

### Diagram zależności

```
Transit.Api  ──────────────────────────┐
     │                                 │
     ▼                                 ▼
Transit.Application  ←─────  Transit.Infrastructure
     │                                 │
     ▼                                 │
Transit.Domain  ◄──────────────────────┘
(zero zewnętrznych zależności)
```

Strzałki wskazują kierunek **zależności** (Domain nic nie wie o reszcie).

---

### 4.1 Transit.Domain

**Lokalizacja:** `backend/src/Transit.Domain/`

Rdzeń domeny — zero zewnętrznych zależności (brak NuGet, brak EF, brak ASP.NET).

**Encje domenowe** (25, pogrupowane według obszaru):

| Obszar | Encje |
|---|---|
| Słowniki | `TypDnia`, `UprawnienieKategorii`, `KategoriaOplaty`, `TypPrzegladu`, `Dzielnica` |
| Pojazdy | `ProducentPojazdu`, `ModelPojazdu`, `Pojazd` |
| Sieć | `Przystanek`, `Linia`, `WariantTrasy`, `PrzystanekWariantu` |
| Personel | `Kierowca`, `UprawnienieKierowcy`, `BadanieLekarskie`, `Kontroler` |
| Rozkłady | `RozkladJazdy`, `OdjazdPlanowy`, `RealizacjaKursu`, `DziennikPrzejazdu`, `GrafikPracy`, `PrzegladUsterka` |
| Pasażerowie | `Pasazer`, `BiletyOkresowe`, `KontrolaWPojedzie`, `Mandat` |

**Konwencje:**
- `private set` na wszystkich właściwościach
- Mutacje wyłącznie przez metody domenowe (np. `Pojazd.Deaktywuj()`, `RealizacjaKursu.ZmienStatus()`)
- Factory methods z walidacją (`Pojazd.Utworz(...)`, `Mandat.Utworz(...)`)
- Wyjątki domenowe: `DomainException` w `Transit.Domain/Exceptions/`
- Stałe statusów: `StatusKursu`, `WynikBadania`, `WynikPrzegladu`, `TypPojazdu`

**Value Object:**
- `Vin` — waliduje format VIN (17 znaków, regex `[A-HJ-NPR-Z0-9]{17}`), immutable, `IEquatable<Vin>`

---

### 4.2 Transit.Application

**Lokalizacja:** `backend/src/Transit.Application/`

CQRS z MediatR. Zależy tylko od Domain i interfejsów — zero konkretnych implementacji.

**Abstrakcje (interfejsy):**

| Interfejs | Opis |
|---|---|
| `IApplicationDbContext` | Ekspozycja wszystkich `DbSet<T>` + `SaveChangesAsync` |
| `IIdentityService` | Rejestracja, logowanie, odświeżanie tokenu, role |
| `IJwtTokenService` | Generowanie access/refresh tokenów, walidacja |
| `IDateTimeProvider` | Mockable „teraz" (`DateTime.UtcNow`, `DateOnly.Today`) |
| `IWyszukiwaczPolaczen` | Serwis routingu połączeń (granica między MVP a przyszłym algorytmem grafowym) |

**MediatR pipeline behaviors** (kolejność wykonania):

1. `LoggingBehavior` — loguje nazwę request i czas wykonania
2. `ValidationBehavior` — FluentValidation, rzuca `ValidationException` przy błędach
3. `UnitOfWorkBehavior` — dla `ICommand` woła `SaveChangesAsync()` po handlerze

**Features (CQRS use cases):**

| Feature | Komendy | Zapytania |
|---|---|---|
| Auth | `LoginCommand`, `RefreshTokenCommand` | — |
| Polaczenia | — | `WyszukajPolaczeniaQuery` |
| Rozklady | — | `PobierzOdjazdyZPrzystankuQuery`, `PobierzRozkladLiniiQuery` |
| Przystanki | `UtworzPrzystanekCommand` | `PobierzPrzystankiQuery` |
| Linie | `UtworzLinieCommand` | `PobierzLinieQuery` |
| Kierowcy | `UtworzKierowceCommand`, `NadajUprawnienieCommand`, `DodajBadanieLekarsieCommand` | `PobierzKierowcowQuery` |
| Pojazdy | `UtworzPojazdCommand` | `PobierzPojazdyQuery` |
| Przeglady | `DodajPrzegladCommand` | — |
| RealizacjeKursow | `UtworzRealizacjeCommand`, `PrzypiszKierowcePojazdCommand` | — |
| Grafiki | `DodajGrafikCommand` | — |
| Kontrole | `ZapiszKontroleCommand`, `WystawMandatCommand` | — |
| Pasazerowie | `UtworzPasazeraCommand` | — |
| Bilety | `WystawnBiletCommand` | — |

**Wspólne modele:** `PaginatedList<T>` (z EF Core `.CountAsync` + `.Skip().Take()`), wyjątki aplikacyjne (`NotFoundException`, `ValidationException`, `ForbiddenException`).

---

### 4.3 Transit.Infrastructure

**Lokalizacja:** `backend/src/Transit.Infrastructure/`

Implementuje wszystkie interfejsy z Application. Jedyna warstwa z dostępem do bazy i zewnętrznych serwisów.

**TransitDbContext** (`Persistence/TransitDbContext.cs`):
- Dziedziczy po `IdentityDbContext<ApplicationUser, ApplicationRole, int>`
- Implementuje `IApplicationDbContext`
- Wszystkie tabele biznesowe w jednej bazie razem z tabelami Identity

**Konfiguracje EF Core** (Fluent API, 6 plików):
- Nazwy tabel 1:1 ze schematem Prismy (polish snake_case: `Typy_Dni`, `Przystanki`, `Rozklady_Jazdy`, ...)
- Indeksy unikalne, wartości domyślne
- Relacje (FK, required/optional)
- Filtered unique index (PESEL i Email w `Pasazerowie` z `WHERE IS NOT NULL`)

**Migracje:**
- `20260512201526_InitialCreate` — pełny schemat (115 operacji DDL)
- `20260512201551_AddConstraintsAndFilteredIndexes` — ręczne SQL:
  - `CREATE UNIQUE INDEX uq_uprawnienie_kierowcy ON "Uprawnienia_Kierowcow" (id_kierowcy, id_uprawnienia)` 
  - `CHECK (status_kursu <> 'zrealizowany' OR (id_kierowcy IS NOT NULL AND id_pojazdu IS NOT NULL))`
  - `CHECK (id_pasazera IS NOT NULL OR nr_dokumentu_pasazera IS NOT NULL)`

**Identity i JWT:**
- `ApplicationUser : IdentityUser<int>` — dodane: `RefreshToken`, `RefreshTokenExpiry`
- `ApplicationRole : IdentityRole<int>`
- Role: `Admin`, `Dyspozytor`, `Kontroler`, `Kierowca`, `Pasazer`
- `JwtTokenService` — HMAC-SHA256, configurowalny TTL access (domyślnie 60 min) i refresh (30 dni)
- `IdentityService` — zarządza pełnym cyklem logowania/refresh

**Routing połączeń:**
- `WyszukiwaczPolaczenJednoliniowy` implementuje `IWyszukiwaczPolaczen`
- MVP: jedna linia, bez przesiadek
- Algorytm: JOIN PrzystankiWariantu (A < B w kolejności) → znajdź odjazdy po zadanej godzinie → dopasuj przyjazd B przez `nr_kursu` i `id_rozkladu`
- Interfejs gotowy na podmianę na algorytm grafowy (CSA/RAPTOR) bez zmian w handlerze

**Seed:**
- `SeedSlownikow` — typy dni (ROB/SOB/SWI), kategorie uprawnień (D/D+E/T), kategorie biletów (normalny/ulgowy/bezpłatny), typy przeglądów
- `SeedDemo` (tylko `Development`) — 2 dzielnice, 5 przystanków, producent + model + 2 pojazdy, 2 kierowców, linia 180, 1 wariant, rozkład roboczy z 3 kursami
- `DbInitializer` — uruchamia migracje, tworzy role, seed słowników i (dev) seed demo + konto `admin@transit.local / Admin123!`

---

### 4.4 Transit.Api

**Lokalizacja:** `backend/src/Transit.Api/`

Cienkie kontrolery — tylko `Sender.Send(query/command)`, bez logiki.

**Kontrolery V1** (`/api/v1/...`):

| Kontroler | Endpointy | Auth |
|---|---|---|
| `AuthController` | `POST /auth/login`, `POST /auth/refresh` | Publiczny |
| `PolaczeniaController` | `GET /polaczenia?przystanekZ&przystanekDo&data&czas&maxWynikow` | Publiczny |
| `RozkladyController` | `GET /przystanki/{id}/odjazdy?okno`, `GET /linie/{id}/rozklad?typDnia` | Publiczny |
| `PrzystankiController` | `GET /przystanki`, `POST /przystanki` | GET: publiczny; POST: Admin/Dyspozytor |
| `LinieController` | `GET /linie`, `POST /linie` | GET: publiczny; POST: Admin/Dyspozytor |
| `KierowcyController` | `GET /kierowcy`, `POST /kierowcy`, `POST /kierowcy/{id}/uprawnienia`, `POST /kierowcy/{id}/badania` | Admin/Dyspozytor |
| `PojazdyController` | `GET /pojazdy`, `POST /pojazdy`, `POST /pojazdy/{id}/przeglady` | Admin/Dyspozytor |
| `RealizacjeController` | `POST /realizacje`, `POST /realizacje/{id}/przypisz` | Admin/Dyspozytor |
| `GrafikiController` | `POST /grafiki` | Admin/Dyspozytor |
| `KontroleController` | `POST /kontrole`, `POST /kontrole/{id}/mandaty` | Admin/Kontroler |
| `PasazerowieController` | `POST /pasazerowie`, `POST /pasazerowie/{id}/bilety` | Admin/Dyspozytor |

**Cross-cutting:**
- `ExceptionHandlingMiddleware` — mapuje wyjątki na ProblemDetails (RFC 7807):
  - `ValidationException` → 400
  - `NotFoundException` → 404
  - `ForbiddenException` → 403
  - `DomainException` → 422
  - reszta → 500 bez wycieku stacktrace
- `ConfigureSwaggerOptions` — Bearer JWT w UI, XML docs, grupowanie po wersji
- CORS — konfigurowalny z `appsettings.json` (dev: `*`, prod: whitelist)
- Wersjonowanie URL: `Asp.Versioning.Mvc` v10, segment `/api/v{version}/`
- Serilog — logowanie na konsolę

---

## 5. Testy

### 5.1 Transit.Domain.UnitTests — 25 testów

Czyste testy logiki domenowej (milisekundy, zero zależności).

| Plik testowy | Co testuje |
|---|---|
| `VinTests` | 4 prawidłowe VIN-y, 4 nieprawidłowe formaty, konwersja na uppercase, równość value objectów |
| `PojazdTests` | `Utworz` z poprawnymi danymi, rok w przyszłości → wyjątek, `Deaktywuj`, `Aktywuj` |
| `RealizacjaKursuTests` | Status „zaplanowany" po utworzeniu, zmiana na „zrealizowany" bez kierowcy → wyjątek, bez pojazdu → wyjątek, z obojgiem → OK |
| `MandatTests` | Bez pasażera i dokumentu → wyjątek, ujemna kwota → wyjątek, poprawny mandat, `ZapiszPlatnosc` |
| `KierowcaTests` | Nowy kierowca jest aktywny, `Deaktywuj`, brak badań → `MaWaznieBadanie` = false, badanie negatywne → `JestWazne` = false |

### 5.2 Transit.Application.UnitTests — 6 testów

Handlery z EF InMemory, walidatory, behaviors.

| Plik testowy | Co testuje |
|---|---|
| `UtworzPrzystanekTests` | Handler tworzy przystanek gdy dzielnica istnieje; rzuca `NotFoundException` gdy nie ma dzielnicy |
| `UtworzKierowceTests` | Handler tworzy kierowcę; walidator odrzuca puste pola |
| `ValidationBehaviorTests` | Behavior przepuszcza poprawne żądanie; rzuca `ValidationException` dla niepoprawnego |

### 5.3 Transit.Api.IntegrationTests — 4 testy

WebApplicationFactory + Testcontainers (Postgres 16 w Dockerze, realny silnik). Jeden kontener dla całej kolekcji `[Collection("Integration")]`.

| Klasa / test | Co testuje |
|---|---|
| `AuthFlowTests.Login_prawidlowe_dane_zwraca_200_i_tokeny` | Tworzy użytkownika przez Identity, loguje, sprawdza access + refresh token w odpowiedzi |
| `AuthFlowTests.Login_zle_haslo_zwraca_400` | Błędne dane → 400 z problemDetails |
| `WyszukajPolaczeniaTests.Wyszukaj_ten_sam_przystanek_zwraca_400` | A == B → walidacja FluentValidation → 400 |
| `WyszukajPolaczeniaTests.Wyszukaj_nieistniejace_przystanki_zwraca_pusta_liste` | Nieistniejące ID → pusta lista 200 |

**Setup fabryki:**
- Testcontainers startuje Postgres przed pierwszym testem w kolekcji
- Migracje uruchamiane bezpośrednio przez `DbContextOptionsBuilder` (nie przez IoC fabryki)
- Seedy słowników uruchamiane przed testami
- Env vars (`ConnectionStrings__DefaultConnection`, `Jwt__*`) ustawiane przed tworzeniem `WebApplicationFactory`
- `ICollectionFixture<CustomWebApplicationFactory>` — jedna fabryka dla wszystkich klas w kolekcji (rozwiązanie konfliktu równoległych fabryk z .NET 10 `HostFactoryResolver`)

### Uruchamianie testów

```bash
cd backend

# Wszystkie (unit + integracyjne, wymaga Dockera)
dotnet test

# Tylko domenowe (~50 ms)
dotnet test tests/Transit.Domain.UnitTests

# Tylko application (~1 s)
dotnet test tests/Transit.Application.UnitTests

# Integracyjne (~30 s, wymaga Docker)
dotnet test tests/Transit.Api.IntegrationTests
```

---

## 6. Baza danych — schemat

25 tabel biznesowych + tabele ASP.NET Identity (`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`, ...).

```
Słowniki:
  Typy_Dni, Uprawnienia_Kategorii, Kategorie_Biletow, Typy_Przegladu, Dzielnice

Pojazdy:
  Producenci_Pojazdow ──< Modele_Pojazdow ──< Pojazdy

Sieć komunikacyjna:
  Dzielnice ──< Przystanki
  Linie ──< Warianty_Trasy ──< Przystanki_Wariantu >── Przystanki
                            └──< Rozklady_Jazdy >── Typy_Dni
                                               └──< Odjazdy_Planowe >── Przystanki_Wariantu

Personel:
  Kierowcy ──< Uprawnienia_Kierowcow >── Uprawnienia_Kategorii
  Kierowcy ──< Badania_Lekarskie
  Kontrolerzy

Realizacja:
  Warianty_Trasy ──< Realizacja_Kursu >── Kierowcy, Pojazdy
  Realizacja_Kursu ──< Dziennik_Przejazdu >── Odjazdy_Planowe
  Realizacja_Kursu ──< Kontrole_W_Pojazdach >── Kontrolerzy
  Kontrole_W_Pojazdach ──< Mandaty >── Pasazerowie (opcjonalnie)
  Kierowcy ──< Grafiki_Pracy >── Pojazdy
  Pojazdy ──< Przeglady_Usterki >── Typy_Przegladu

Pasażerowie:
  Pasazerowie ──< Bilety_Okresowe >── Kategorie_Biletow
  Pasazerowie ──< Mandaty
```

**Ograniczenia w bazie (ręczne SQL):**
- `uq_uprawnienie_kierowcy` — jeden rekord uprawnienia per kierowca per kategoria
- `CHECK (status_kursu <> 'zrealizowany' OR (id_kierowcy IS NOT NULL AND id_pojazdu IS NOT NULL))`
- `CHECK (id_pasazera IS NOT NULL OR nr_dokumentu_pasazera IS NOT NULL)`

---

## 7. Jak uruchomić lokalnie

```bash
# Klonowanie
git clone git@github.com:dawidzygmunt/GdzieToSieJechalo.git
cd GdzieToSieJechalo/backend

# Baza danych
docker compose up -d
# PostgreSQL: localhost:5432
# pgAdmin:    http://localhost:5050  (admin@transit.local / admin)

# Migracje (export PATH jeśli dotnet-ef nie jest w PATH)
export PATH="$PATH:$HOME/.dotnet/tools"
dotnet ef database update --project src/Transit.Infrastructure --startup-project src/Transit.Api

# Uruchomienie API
dotnet run --project src/Transit.Api
# → http://localhost:5000/swagger

# Konto admina z seedu demo:
# Email: admin@transit.local
# Hasło: Admin123!

# Testy
dotnet test
```

---

## 8. Konfiguracja (`backend/src/Transit.Api/appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=transit_db;Username=transit_user;Password=transit_pass"
  },
  "Jwt": {
    "SecretKey": "ZMIEŃ_PRZED_PRODUKCJĄ_MIN_32_ZNAKI!",
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

---

## 9. Co jeszcze NIE jest zrobione (świadome braki MVP)

### Backend

| Brakujący element | Priorytet | Uwagi |
|---|---|---|
| Wyszukiwanie połączeń z **przesiadkami** | Wysoki | Interfejs `IWyszukiwaczPolaczen` gotowy; wymaga algorytmu CSA/RAPTOR lub Dijkstry na grafie czasowym |
| Endpointy **GET szczegółów** (kierowca, pojazd, linia, realizacja...) | Wysoki | Są tylko listy i POST; brak `GET /kierowcy/{id}`, `PUT`, `DELETE` |
| **Rejestracja pasażera z JWT** | Średni | Pasażer tworzony jest przez admina; brak self-registration |
| **Kalendarz dni wyjątkowych** | Średni | Typ dnia wyznaczany prostą regułą (pn-pt/sb/nd); brak obsługi świąt |
| **Soft delete** | Niski | Flaga `aktywny` jest, ale `DELETE` endpoint nie istnieje |
| **Cache** (Redis) | Niski | Częste query rozkładów mogą trafić w cache |
| **Health checks** | Niski | `/health` endpoint — wymagany przed deploy na k8s/ECS |
| **OpenTelemetry / tracing** | Niski | Serilog jest, brak distributed tracing |

### Monorepo

| Element | Status |
|---|---|
| `panel/` — React/Next.js panel zarządzania | Placeholder — nie zaczęty |
| `mobile/` — React Native / Flutter aplikacja mobilna | Placeholder — nie zaczęty |
| CI/CD (GitHub Actions) | Brak — do dodania |
| Dockerfile dla API | Brak — do dodania |

---

## 10. Kluczowe decyzje techniczne i dlaczego

| Decyzja | Uzasadnienie |
|---|---|
| Clean Architecture zamiast prostego MVC | Testability (testy domenowe bez EF/ASP.NET), łatwiejsza wymiana infrastruktury (np. ORM) |
| CQRS + MediatR zamiast serwisów | Każda operacja to osobny handler — łatwe testowanie w izolacji, czytelny punkt wejścia do logiki |
| `IWyszukiwaczPolaczen` jako osobny interfejs | Oddzielenie MVP (jedna linia) od przyszłego algorytmu grafowego — handler nie zmienia się przy zmianie implementacji |
| EF Core zamiast Dappera | Migracje, type-safety, LINQ — przy tej domenie (złożone relacje) EF wygrywa; Dapper do raportów w przyszłości |
| `IClassFixture` → `ICollectionFixture` w testach | .NET 10 `HostFactoryResolver` nie obsługuje poprawnie dwóch równoległych fabryk w tym samym procesie |
| Seed demo w `Development` | Umożliwia natychmiastowe testowanie Swaggera bez ręcznego dodawania danych |
| Polskie nazwy encji domenowych | Język domeny (komunikacja miejska PL) — ułatwia komunikację z developerem i mapowanie na wymagania; C# API publiczne w stylu PL |
