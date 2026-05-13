# Domyślne konto administratora

## Dane logowania

Po uruchomieniu aplikacji automatycznie tworzone jest domyślne konto administratora:

- **Email:** `admin@transit.local`
- **Hasło:** `Admin123!`
- **Rola:** Admin (pełne uprawnienia)

## Jak się zalogować

### 1. Uruchom backend
```bash
cd backend
dotnet run --project src/Transit.Api/Transit.Api.csproj
```

### 2. Uruchom frontend
```bash
cd panel
npm run dev
```

### 3. Zaloguj się
1. Otwórz przeglądarkę: http://localhost:3000 (lub 3001)
2. Przejdź do strony logowania
3. Wprowadź dane:
   - Email: `admin@transit.local`
   - Hasło: `Admin123!`
4. Kliknij "Zaloguj"

## Uprawnienia administratora

Konto admina ma dostęp do wszystkich funkcji systemu:
- ✅ Zarządzanie kierowcami
- ✅ Zarządzanie pojazdami
- ✅ Zarządzanie liniami
- ✅ Zarządzanie przystankami
- ✅ Zarządzanie rozkładami jazdy
- ✅ Zarządzanie realizacjami kursów
- ✅ Zarządzanie użytkownikami (w przyszłości)

## Automatyczne tworzenie konta

Konto administratora jest tworzone automatycznie przy:
- **Pierwszym uruchomieniu aplikacji**
- **Każdej migracji bazy danych**

Jeśli konto już istnieje, system:
- Nie tworzy duplikatu
- Sprawdza czy użytkownik ma rolę Admin
- Dodaje rolę Admin jeśli jej brakuje

## Zmiana hasła (w przyszłości)

Po pierwszym zalogowaniu zaleca się zmianę hasła na bardziej bezpieczne.

Funkcja zmiany hasła będzie dostępna w:
- Ustawieniach profilu użytkownika
- Panel administracyjny > Moje konto

## Resetowanie konta

Jeśli zapomnisz hasła lub chcesz zresetować konto:

### Metoda 1: Przez bazę danych (development)
```sql
-- Usuń użytkownika z bazy
DELETE FROM "AspNetUserRoles" WHERE "UserId" IN (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'admin@transit.local');
DELETE FROM "AspNetUsers" WHERE "Email" = 'admin@transit.local';

-- Uruchom ponownie aplikację - konto zostanie utworzone automatycznie
```

### Metoda 2: Przez EF Core Migrations (development)
```bash
cd backend
dotnet ef database drop --project src/Transit.Infrastructure
dotnet run --project src/Transit.Api/Transit.Api.csproj
```

## Bezpieczeństwo

### Development
W trybie development domyślne konto jest akceptowalne dla celów testowych.

### Production
⚠️ **WAŻNE:** Przed wdrożeniem na produkcję:

1. **Zmień hasło domyślne** na silne i unikalne
2. **Wyłącz automatyczne seedowanie** w production (opcjonalnie)
3. **Używaj zmiennych środowiskowych** dla danych admina
4. **Włącz 2FA** (jeśli dostępne)
5. **Monitoruj logi logowania** administratora

### Przykład konfiguracji przez zmienne środowiskowe:

```csharp
// W DbInitializer.cs
private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
{
    var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@transit.local";
    var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "Admin123!";

    // ... reszta kodu
}
```

Następnie ustaw zmienne środowiskowe:
```bash
export ADMIN_EMAIL="twoj-email@example.com"
export ADMIN_PASSWORD="TwojeSilneHaslo123!@#"
```

## Testowanie API z cURL

```bash
# 1. Zaloguj się
curl -X POST http://localhost:5210/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@transit.local",
    "password": "Admin123!"
  }'

# Odpowiedź zawiera accessToken i refreshToken
# Skopiuj accessToken

# 2. Użyj tokenu do wywołań API
curl -X GET http://localhost:5210/api/v1/kierowcy \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN_HERE"

# 3. Dodaj kierowcę
curl -X POST http://localhost:5210/api/v1/kierowcy \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN_HERE" \
  -d '{
    "imie": "Jan",
    "nazwisko": "Kowalski",
    "nrPracownika": "12345",
    "dataZatrudnienia": "2024-01-15"
  }'
```

## Dodatkowe konta (Development)

W trybie development tworzone są również przykładowe dane:
- Kierowcy: Jan Kowalski, Anna Nowak
- Pojazdy: Solaris Urbino 12 (1001, 1002)
- Linie: 180 (Centrum - Mokotów)
- Przystanki: Centrum, Teatr Wielki, Plac Unii, Wiśniowa, Politechnika

Te dane są tworzone tylko w development i służą do testowania.

## Rozwiązywanie problemów

### Nie mogę się zalogować
1. Sprawdź czy backend jest uruchomiony
2. Sprawdź czy używasz prawidłowego emaila i hasła
3. Sprawdź logi backendu czy jest informacja o błędzie
4. Sprawdź czy baza danych jest poprawnie zmigrowana

### Konto nie ma uprawnień admina
1. Sprawdź w bazie danych czy rola Admin istnieje:
   ```sql
   SELECT * FROM "AspNetRoles";
   ```
2. Sprawdź czy użytkownik ma przypisaną rolę:
   ```sql
   SELECT * FROM "AspNetUserRoles"
   WHERE "UserId" = (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'admin@transit.local');
   ```
3. Jeśli brak roli, zrestartuj aplikację - kod automatycznie doda brakującą rolę

### Token wygasa zbyt szybko
Token jest ważny przez 60 minut. Po wygaśnięciu:
- Frontend automatycznie odświeża token używając refreshToken
- Jeśli refreshToken również wygasł, musisz zalogować się ponownie

## Kontakt

W razie problemów sprawdź:
- README projektu
- Logi aplikacji
- Console przeglądarki (F12)
- [GitHub Issues](https://github.com/your-repo/issues) (jeśli dotyczy)
