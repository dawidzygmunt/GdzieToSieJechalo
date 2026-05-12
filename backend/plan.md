# Plan: Backend systemu komunikacji miejskiej (.NET 10 + Clean Architecture)

## Context

Tworzymy od zera backend dla aplikacji w stylu „JakDojadę" dla komunikacji miejskiej. Wymagania funkcjonalne:

1. **Wyszukiwanie połączeń A → B** (MVP: jedna linia, bez przesiadek — z miejscem na przyszły algorytm grafowy).
2. **Rozkłady jazdy** — odjazdy z przystanku, rozkład wariantu trasy, rozkład linii dla typu dnia.
3. **Panel zarządzania** — kierowcy, pojazdy, linie/warianty trasy, przystanki, rozkłady, realizacja kursów, grafiki pracy, przeglądy techniczne, kontrolerzy, mandaty, pasażerowie i bilety okresowe.
4. **Aplikacja mobilna** w przyszłości → API musi być czysto RESTowe, z JWT + refresh, CORS, wersjonowaniem i kompaktowymi DTO.

Schemat domenowy (Prisma) dostarczony przez użytkownika — odwzorowujemy go 1:1 na EF Core, zachowując polskie nazwy bytów (język domeny).

**Decyzje techniczne ustalone z użytkownikiem:**
- .NET 10 + PostgreSQL (Npgsql + EF Core 10)
- Clean Architecture (Domain → Application → Infrastructure → API)
- CQRS + MediatR
- ASP.NET Identity + JWT (z refresh tokenami)
- Routing MVP (jedna linia, bez przesiadek) z architekturą gotową na rozszerzenie
- Testy integracyjne: Testcontainers (realny Postgres w Dockerze)
- Mały seed demo

---

## 1. Struktura solucji

```
StudiaOOP/
├── Transit.sln
├── docker-compose.yml                 # Postgres + (opcjonalnie) pgAdmin
├── .editorconfig
├── Directory.Packages.props           # centralne zarządzanie wersjami NuGet
├── README.md
├── src/
│   ├── Transit.Domain/                # ⬅ rdzeń, zero zależności
│   ├── Transit.Application/           # ⬅ zależy od Domain
│   ├── Transit.Infrastructure/        # ⬅ zależy od Application
│   └── Transit.Api/                   # ⬅ zależy od Application + Infrastructure (composition root)
└── tests/
    ├── Transit.Domain.UnitTests/
    ├── Transit.Application.UnitTests/
    └── Transit.Api.IntegrationTests/
```

**Reguła zależności (Clean Architecture):** strzałki idą do środka. Domain nic nie wie o EF Core, ASP.NET, MediatR. Application zna tylko interfejsy (IApplicationDbContext, IJwtTokenService, IDateTimeProvider). Infrastructure i API implementują.

---

## 2. Warstwa Domain (`src/Transit.Domain`)

Encje 1:1 z dostarczonego schematu Prismy, z drobnymi adaptacjami pod C#:

**Słowniki:** `TypDnia`, `UprawnienieKategorii`, `KategoriaOplaty`, `TypPrzegladu`, `Dzielnica`, `ProducentPojazdu`.

**Pojazdy:** `ModelPojazdu`, `Pojazd`.

**Sieć:** `Przystanek`, `Linia`, `WariantTrasy`, `PrzystanekWariantu` (tabela łącząca z `kolejnosc` i `OdleglosOdPoczatkuM`).

**Personel:** `Kierowca`, `UprawnienieKierowcy`, `BadanieLekarskie`, `Kontroler`.

**Rozkład i realizacja:** `RozkladJazdy`, `OdjazdPlanowy`, `RealizacjaKursu`, `DziennikPrzejazdu`, `GrafikPracy`, `PrzegladUsterka`.

**Pasażerowie:** `Pasazer`, `BiletyOkresowe`, `KontrolaWPojedzie`, `Mandat`.

**Konwencje:**
- Wszystkie klasy w `Transit.Domain/Entities/<obszar>/`.
- ID-y jako `int` (zgodnie z Prismą `autoincrement()`).
- Niezmienniki w konstruktorach + factory methods (np. `Pojazd.Utworz(...)` walidujący VIN i rok produkcji).
- `private set` na właściwościach, mutacje przez metody domenowe (`Pojazd.Deaktywuj()`, `Kierowca.NadajUprawnienie(...)`).
- Wartości domenowe (np. `Vin`, `NrBoczny`, `NrPracownika`) jako value objecty w `Transit.Domain/ValueObjects/` — walidacja w konstruktorze.
- Stałe statusów (`StatusKursu`, `WynikBadania`, `WynikKontroli`) jako klasy z `public const string` lub `static readonly` — unikamy rozproszenia magic stringów.
- Wyjątki domenowe w `Transit.Domain/Exceptions/` (np. `DomenowyException`, `NieprawidlowyVinException`).

---

## 3. Warstwa Application (`src/Transit.Application`)

**Struktura folderów (Vertical-flavoured CQRS w ramach Clean Architecture):**

```
Transit.Application/
├── Abstractions/
│   ├── Persistence/
│   │   └── IApplicationDbContext.cs       # ekspozycja DbSet<T> + SaveChangesAsync
│   ├── Identity/
│   │   ├── IIdentityService.cs            # rejestracja, logowanie, role
│   │   └── IJwtTokenService.cs            # access + refresh
│   ├── Time/
│   │   └── IDateTimeProvider.cs           # mockable „teraz"
│   └── Routing/
│       └── IWyszukiwaczPolaczen.cs        # serwis routingu (MVP impl. w Infrastructure)
├── Common/
│   ├── Behaviors/                         # MediatR pipeline (Validation, Logging, UnitOfWork)
│   ├── Mappings/                          # Mapster konfiguracja
│   ├── Models/                            # Result<T>, PaginatedList<T>
│   └── Exceptions/                        # ValidationException, NotFoundException, ForbiddenException
├── Features/
│   ├── Polaczenia/
│   │   └── Queries/
│   │       └── WyszukajPolaczenia/
│   │           ├── WyszukajPolaczeniaQuery.cs
│   │           ├── WyszukajPolaczeniaHandler.cs
│   │           ├── WyszukajPolaczeniaValidator.cs
│   │           └── PolaczenieDto.cs
│   ├── Rozklady/                          # tablica odjazdów z przystanku, rozkład linii
│   ├── Linie/                             # CRUD linii + wariantów
│   ├── Przystanki/                        # CRUD + lista wg dzielnicy
│   ├── Kierowcy/                          # CRUD, uprawnienia, badania lekarskie
│   ├── Pojazdy/                           # CRUD pojazdów i modeli
│   ├── RealizacjeKursow/                  # przypisanie kierowcy+pojazdu, status, dziennik
│   ├── Grafiki/                           # planowanie zmian
│   ├── Przeglady/                         # przeglądy techniczne pojazdów
│   ├── Pasazerowie/                       # rejestracja, dane
│   ├── Bilety/                            # bilety okresowe
│   ├── Kontrolerzy/
│   ├── Kontrole/                          # zapisy kontroli + mandaty
│   └── Auth/                              # rejestracja, login, refresh, role
└── DependencyInjection.cs                 # AddApplication() — MediatR, Validators, Mapster, Behaviors
```

**Każdy feature** ma tę samą strukturę: `Commands/<Akcja>/{Command,Handler,Validator,Dto}.cs` i `Queries/<Pytanie>/{Query,Handler,Dto}.cs`.

**MediatR pipeline behaviors:**
- `ValidationBehavior<TRequest,TResponse>` — FluentValidation, rzuca `ValidationException`.
- `LoggingBehavior` — `ILogger`, mierzy czas.
- `UnitOfWorkBehavior` — dla `ICommand` woła `SaveChangesAsync()` po handlerze (queries nie zapisują).

**Walidacja:** FluentValidation, jeden walidator per command/query.

**Mapowanie:** Mapster (lżejszy i szybszy niż AutoMapper).

---

## 4. Warstwa Infrastructure (`src/Transit.Infrastructure`)

```
Transit.Infrastructure/
├── Persistence/
│   ├── TransitDbContext.cs                # implementuje IApplicationDbContext + IdentityDbContext<ApplicationUser>
│   ├── Configurations/                    # IEntityTypeConfiguration<T> per encja (Fluent API)
│   │   ├── PojazdConfiguration.cs
│   │   ├── PrzystanekConfiguration.cs
│   │   └── ...                            # po jednej na encję — nazwy tabel, indeksy unikalne, CHECK
│   ├── Migrations/
│   ├── Interceptors/
│   │   └── AuditableEntityInterceptor.cs  # auto-uzupełnia CreatedAt/UpdatedAt jeśli wprowadzimy IAuditable
│   └── Seeding/
│       ├── DbInitializer.cs               # uruchamia migracje + seed na starcie (dev)
│       ├── SeedSlownikow.cs               # TypyDni, Kategorie, Uprawnienia, TypyPrzegladu
│       └── SeedDemo.cs                    # przykładowe dzielnice, przystanki, linie, kierowcy, rozkład
├── Identity/
│   ├── ApplicationUser.cs                 # IdentityUser<int>
│   ├── ApplicationRole.cs                 # IdentityRole<int>
│   ├── Roles.cs                           # const: Admin, Dyspozytor, Kontroler, Kierowca, Pasazer
│   ├── IdentityService.cs                 # implementacja IIdentityService
│   └── JwtTokenService.cs                 # access + refresh, klucze z appsettings
├── Routing/
│   └── WyszukiwaczPolaczenJednoliniowy.cs # MVP implementacja IWyszukiwaczPolaczen
├── Time/
│   └── SystemDateTimeProvider.cs
└── DependencyInjection.cs                 # AddInfrastructure(config)
```

**TransitDbContext** dziedziczy po `IdentityDbContext<ApplicationUser, ApplicationRole, int>` i implementuje `IApplicationDbContext`. Wszystkie tabele biznesowe i identity w jednej bazie.

**Mapowanie nazw tabel:** Fluent API w configurations odzwierciedla nazwy z `@@map(...)` w Prismie (`Pojazdy`, `Przystanki`, `Rozklady_Jazdy` itd.) — gwarantuje, że jeśli kiedyś podmienimy backend, baza pozostaje kompatybilna.

**Ręczne SQL z Prismy (wymagane w migracjach):**
- Filtered unique index na `Uprawnienia_Kierowcow` (jedno aktywne uprawnienie kategorii per kierowca).
- `CHECK (status_kursu <> 'zrealizowany' OR (id_kierowcy IS NOT NULL AND id_pojazdu IS NOT NULL))` na `Realizacja_Kursu`.
- `CHECK (id_pasazera IS NOT NULL OR nr_dokumentu_pasazera IS NOT NULL)` na `Mandaty`.

Realizacja: w jednej z migracji `migrationBuilder.Sql("...")`.

---

## 5. Warstwa API (`src/Transit.Api`)

```
Transit.Api/
├── Program.cs                             # composition root, Swagger, Auth, CORS, Serilog
├── appsettings.json / appsettings.Development.json
├── Controllers/
│   ├── V1/
│   │   ├── PolaczeniaController.cs        # GET /api/v1/polaczenia?z=&do=&kiedy=
│   │   ├── RozkladyController.cs          # GET /api/v1/przystanki/{id}/odjazdy, /api/v1/linie/{id}/rozklad
│   │   ├── LinieController.cs             # CRUD [Admin/Dyspozytor]
│   │   ├── PrzystankiController.cs        # CRUD + lista publiczna
│   │   ├── KierowcyController.cs          # CRUD + uprawnienia + badania
│   │   ├── PojazdyController.cs           # CRUD + przeglądy
│   │   ├── RealizacjeController.cs        # przypisania, status, dziennik
│   │   ├── GrafikiController.cs
│   │   ├── KontroleController.cs          # + mandaty
│   │   ├── PasazerowieController.cs
│   │   ├── BiletyController.cs
│   │   └── AuthController.cs              # POST /register, /login, /refresh
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs     # mapuje wyjątki na ProblemDetails (RFC 7807)
├── Swagger/
│   ├── ConfigureSwaggerOptions.cs         # JWT scheme, XML docs, grupowanie wg wersji
│   └── PolskieDescriptions.cs             # operacje + opisy
└── Filters/
    └── ValidationFilter.cs                # spinanie Application ValidationException -> 400
```

**Kontrolery są cienkie:** tylko `_sender.Send(query)` + zwrot wyniku. Cała logika w handlerach.

**Swagger:**
- Swashbuckle.AspNetCore + Swashbuckle.AspNetCore.Annotations.
- Włączone XML doc comments (`<GenerateDocumentationFile>true</GenerateDocumentationFile>` w `.csproj`).
- Bearer JWT w UI (przycisk „Authorize").
- Grupowanie po wersji (v1).
- Dostępny pod `/swagger` w dev; w produkcji za polityką (np. ukryty/za auth).

**Autoryzacja:**
- Polityki: `WymagaAdmin`, `WymagaDyspozytor`, `WymagaKontroler`, `WymagaPersonel` (Admin∪Dyspozytor∪Kontroler).
- Endpointy publiczne (mobile): wyszukiwanie połączeń, rozkłady, lista przystanków/linii — `[AllowAnonymous]`.

**Wersjonowanie:** `Asp.Versioning.Mvc` — segment URL `/api/v{version:apiVersion}/...`.

**Logowanie:** Serilog → konsola + plik; format JSON w produkcji.

**Globalna obsługa błędów:** `ExceptionHandlingMiddleware` mapuje:
- `ValidationException` → 400 + lista błędów.
- `NotFoundException` → 404.
- `ForbiddenException` → 403.
- Pozostałe → 500 z trace id (bez wycieku stacktrace na prod).

**Mobile-friendliness:**
- CORS skonfigurowany przez `Cors:AllowedOrigins` z appsettings (na dev `*`, na prod whitelist).
- Wszystkie listy paginowane (`?page=&pageSize=`), max page size 100.
- ProblemDetails jednolite.
- Refresh token endpoint zapobiega ciągłemu pytaniu o hasło.

---

## 6. Kluczowy use case: wyszukiwanie połączeń (MVP)

**Endpoint:** `GET /api/v1/polaczenia?przystanekZ={idA}&przystanekDo={idB}&data={yyyy-MM-dd}&czas={HH:mm}&maxWynikow=10`

**Algorytm (`WyszukajPolaczeniaHandler` + `WyszukiwaczPolaczenJednoliniowy`):**

1. Wyznacz `typDnia` (`TypDnia` z dnia tygodnia / kalendarza świąt — na MVP: pn-pt = roboczy, sb, nd/święta).
2. Zapytanie SQL (LINQ na EF Core):
   ```
   SELECT wariant, przystanekZ_PW, przystanekDo_PW
   FROM PrzystankiWariantu pwA
   JOIN PrzystankiWariantu pwB ON pwA.IdWariantu = pwB.IdWariantu
                                AND pwA.Kolejnosc < pwB.Kolejnosc
   JOIN WariantyTrasy w ON w.IdWariantu = pwA.IdWariantu AND w.Aktywny
   JOIN Linie l ON l.IdLinii = w.IdLinii AND l.Aktywna
   WHERE pwA.IdPrzystanku = @idA AND pwB.IdPrzystanku = @idB
   ```
3. Dla każdego wariantu znajdź `OdjazdyPlanowe` z `RozkladuJazdy` ważnego dla `data` i `typDnia`, gdzie `planowa_godzina_odjazdu >= czas`, z przystanku A.
4. Czas dotarcia do B = czas odjazdu z A + (czas odjazdu B - czas odjazdu A) z tego samego `nr_kursu`.
5. Zwróć posortowane po godzinie odjazdu, top N.

**DTO `PolaczenieDto`:** numer linii, kierunek, przystanek z/do, godzina odjazdu, godzina przyjazdu, czas trwania, liczba przystanków pośrednich.

**Interfejs `IWyszukiwaczPolaczen` w Application** — implementacja w Infrastructure. Gdy w przyszłości dodamy algorytm grafowy z przesiadkami, podmienimy implementację bez ruszania handlera.

---

## 7. Inne kluczowe use case'y (skrót)

- **Tablica odjazdów z przystanku:** `GET /api/v1/przystanki/{id}/odjazdy?okno=60` — najbliższe odjazdy w oknie czasowym (mobile-friendly).
- **Rozkład linii:** `GET /api/v1/linie/{id}/rozklad?typDnia=ROB` — pełna tabela.
- **CRUD kierowcy** + sub-zasoby: `POST /api/v1/kierowcy/{id}/uprawnienia`, `POST /api/v1/kierowcy/{id}/badania`. Walidacja: nie można przypisać kierowcy do realizacji bez ważnego badania i uprawnienia odpowiedniej kategorii (sprawdzane w `PrzypiszKierowceDoRealizacjiHandler`).
- **Przeglądy pojazdu:** automatyczne wyliczanie kolejnego terminu na podstawie `interwal_dni` z `TypPrzegladu`.
- **Mandat:** wymagane FK do `Pasazer` lub `nr_dokumentu_pasazera` (CHECK w bazie + walidator).
- **Realizacja kursu = "zrealizowany"** wymaga obu: kierowca + pojazd (CHECK + walidator).

---

## 8. Testy

### `tests/Transit.Domain.UnitTests`
- xUnit + FluentAssertions.
- Testy value objectów (`VinTests`: poprawne/niepoprawne VIN-y, długość 17, dozwolone znaki).
- Testy invariantów encji (`PojazdTests.Utworz_RokWPrzyszlosci_RzucaWyjatek`).

### `tests/Transit.Application.UnitTests`
- xUnit + FluentAssertions + NSubstitute.
- Testy handlerów z `IApplicationDbContext` zamockowanym przez EF Core InMemory provider (jako szybki test logiki, nie SQL-a).
- Testy walidatorów.
- Testy MediatR behaviors (Validation, UnitOfWork).
- Kluczowe testy: `WyszukajPolaczeniaHandlerTests` — różne scenariusze (brak wariantu, A po B, kursy w przeszłości, granice typu dnia).

### `tests/Transit.Api.IntegrationTests`
- xUnit + `WebApplicationFactory<Program>` + **Testcontainers.PostgreSql**.
- Każda klasa testowa = świeży kontener Postgres (xUnit `IAsyncLifetime`).
- `CustomWebApplicationFactory` podmienia connection string na container.GetConnectionString().
- Po starcie: migracje + seed minimalnych słowników.
- Scenariusze:
  - **AuthFlow**: register → login → refresh → wywołanie chronione.
  - **WyszukiwanieFlow**: setup linii/wariantu/rozkładu → query polaczen → poprawny wynik.
  - **PanelKierowcyFlow**: utwórz kierowcę, nadaj uprawnienie, próba przypisania bez ważnego badania → 400.
  - **CRUD-y**: szybkie smoke testy każdego kontrolera (POST/GET/PUT/DELETE).
- Asercje na statusach HTTP + zawartości JSON (System.Text.Json + FluentAssertions).

### Uruchamianie
```
dotnet test                              # wszystkie
dotnet test tests/Transit.Domain.UnitTests
dotnet test tests/Transit.Api.IntegrationTests   # wymaga Dockera
```

---

## 9. NuGet — kluczowe paczki (zarządzane centralnie w `Directory.Packages.props`)

- **EF Core 10**: `Microsoft.EntityFrameworkCore`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.Design`, `Microsoft.EntityFrameworkCore.Tools`.
- **Identity**: `Microsoft.AspNetCore.Identity.EntityFrameworkCore`.
- **JWT**: `Microsoft.AspNetCore.Authentication.JwtBearer`.
- **CQRS**: `MediatR`.
- **Walidacja**: `FluentValidation`, `FluentValidation.DependencyInjectionExtensions`.
- **Mapping**: `Mapster`, `Mapster.DependencyInjection`.
- **Swagger**: `Swashbuckle.AspNetCore`, `Swashbuckle.AspNetCore.Annotations`.
- **Wersjonowanie**: `Asp.Versioning.Mvc`, `Asp.Versioning.Mvc.ApiExplorer`.
- **Logowanie**: `Serilog.AspNetCore`, `Serilog.Sinks.Console`.
- **Testy**: `xunit`, `xunit.runner.visualstudio`, `FluentAssertions`, `NSubstitute`, `Microsoft.AspNetCore.Mvc.Testing`, `Testcontainers.PostgreSql`, `Microsoft.EntityFrameworkCore.InMemory` (tylko w Application.UnitTests).

---

## 10. Kolejność implementacji

1. **Setup solucji** — `dotnet new sln`, 4 projekty src + 3 tests, referencje między projektami, `Directory.Packages.props`, `.editorconfig`.
2. **docker-compose.yml** z Postgresem + `appsettings.Development.json` z connection stringiem.
3. **Domain** — encje, value objecty, wyjątki domenowe (jeden plik na encję/grupę).
4. **Infrastructure: DbContext + konfiguracje** — wszystkie `IEntityTypeConfiguration<T>`, mapowanie nazw tabel z `@@map`.
5. **Migracja initial** + ręczne SQL (filtered unique index, CHECK-i) + `dotnet ef database update`.
6. **Identity + JWT** — `ApplicationUser`, role, `IIdentityService`, `JwtTokenService`, AuthController z register/login/refresh.
7. **Seed słowników + demo** — `DbInitializer` woła `SeedSlownikow` i (jeśli `Environment=Development`) `SeedDemo`.
8. **MediatR + Behaviors + DI** — Application/Infrastructure `DependencyInjection.cs`.
9. **Feature: Wyszukiwanie połączeń** — handler + serwis + controller + walidator + DTO + testy.
10. **Feature: Rozkłady** — tablica odjazdów, rozkład linii.
11. **CRUD-y panelu** w kolejności: Przystanki → Linie/Warianty → Pojazdy → Kierowcy → RealizacjeKursow → Grafiki → Przeglądy → Kontrolerzy/Kontrole/Mandaty → Pasażerowie → Bilety.
12. **Swagger** z JWT + XML docs.
13. **Middleware błędów** + ProblemDetails.
14. **Testy integracyjne** — `CustomWebApplicationFactory` z Testcontainers, scenariusze z punktu 8.
15. **README** — jak uruchomić (docker-compose up, dotnet ef database update, dotnet run, /swagger, dotnet test).

---

## 11. Pliki krytyczne (do utworzenia)

Najważniejsze, których kształt determinuje resztę:

- `Transit.sln` — root.
- [Directory.Packages.props](Directory.Packages.props) — centralne wersje.
- [src/Transit.Application/Abstractions/Persistence/IApplicationDbContext.cs](src/Transit.Application/Abstractions/Persistence/IApplicationDbContext.cs) — kontrakt DbContextu.
- [src/Transit.Application/Abstractions/Routing/IWyszukiwaczPolaczen.cs](src/Transit.Application/Abstractions/Routing/IWyszukiwaczPolaczen.cs) — granica między MVP a przyszłym algorytmem grafowym.
- [src/Transit.Infrastructure/Persistence/TransitDbContext.cs](src/Transit.Infrastructure/Persistence/TransitDbContext.cs).
- [src/Transit.Infrastructure/Routing/WyszukiwaczPolaczenJednoliniowy.cs](src/Transit.Infrastructure/Routing/WyszukiwaczPolaczenJednoliniowy.cs).
- [src/Transit.Api/Program.cs](src/Transit.Api/Program.cs) — composition root.
- [src/Transit.Api/Controllers/V1/PolaczeniaController.cs](src/Transit.Api/Controllers/V1/PolaczeniaController.cs).
- [tests/Transit.Api.IntegrationTests/CustomWebApplicationFactory.cs](tests/Transit.Api.IntegrationTests/CustomWebApplicationFactory.cs).
- [docker-compose.yml](docker-compose.yml).

---

## 12. Weryfikacja (end-to-end)

```bash
# 1. Postgres w Dockerze
docker compose up -d

# 2. Migracje + seed (DbInitializer odpala się też z Program.cs na dev)
dotnet ef database update --project src/Transit.Infrastructure --startup-project src/Transit.Api

# 3. Uruchom API
dotnet run --project src/Transit.Api

# 4. Swagger
# Otwórz http://localhost:5000/swagger — powinieneś zobaczyć wszystkie endpointy v1.

# 5. Smoke test ręczny (curl / Swagger):
#    a) POST /api/v1/auth/register  -> stwórz admina
#    b) POST /api/v1/auth/login     -> dostań JWT
#    c) GET  /api/v1/polaczenia?przystanekZ=1&przystanekDo=5&data=2026-05-13&czas=08:00
#       -> powinno zwrócić listę połączeń z seedu demo
#    d) GET  /api/v1/przystanki/1/odjazdy  -> tablica odjazdów

# 6. Testy
dotnet test                                       # wszystkie zielone
dotnet test tests/Transit.Domain.UnitTests        # ~milisekundy
dotnet test tests/Transit.Application.UnitTests   # ~sekundy
dotnet test tests/Transit.Api.IntegrationTests    # ~minuta (kontener Postgres)
```

**Kryteria akceptacji:**
- Swagger pokazuje wszystkie endpointy z opisami i JWT auth działa z poziomu UI.
- Wyszukiwanie połączeń zwraca poprawne wyniki na danych demo.
- Wszystkie testy zielone.
- Endpointy panelu admina wymagają roli i zwracają 401/403 dla anonimowych/błędnych ról.
- Niezmienniki domenowe (CHECK-i, filtered unique) działają — odpowiednie testy integracyjne to potwierdzają.

---

## 13. Co odkładamy na później (świadomie poza MVP)

- Algorytm grafowy z przesiadkami (Dijkstra na grafie czasowo-zależnym lub CSA/RAPTOR) — interfejs `IWyszukiwaczPolaczen` jest już na to gotowy.
- GTFS import/eksport.
- WebSockets / SignalR do live-trackingu pojazdów.
- Push notyfikacje dla mobile (FCM).
- Cache Redis dla często czytanych rozkładów.
- Health checks / observability (OpenTelemetry).

Te punkty NIE są częścią pierwszej iteracji — wymieniam je tylko jako mapę drogową, żeby było jasne, co zostawiamy „na potem".
