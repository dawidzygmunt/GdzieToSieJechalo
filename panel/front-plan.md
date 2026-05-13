# Plan: Panel Zarządzania React dla GdzieToSieJechalo

## Podsumowanie

Panel administracyjny w React do zarządzania systemem komunikacji miejskiej. Nowoczesny, prosty, profesjonalny design ze stonowaną paletą kolorów (niebieski/slate/teal).

---

## 1. Stack Technologiczny

| Technologia | Przeznaczenie |
|-------------|---------------|
| **Vite + React 18 + TypeScript** | Framework |
| **TanStack Query v5** | Cachowanie danych API |
| **Zustand** | Stan globalny (auth, UI) |
| **React Router v6** | Routing |
| **shadcn/ui + Tailwind CSS** | Komponenty UI |
| **React Hook Form + zod** | Formularze i walidacja |
| **axios** | HTTP client z interceptorami |
| **lucide-react** | Ikony |

---

## 2. Paleta Kolorystyczna

Stonowana, profesjonalna paleta inspirowana skandynawskim minimalizmem:

- **Primary**: Sky blue (#0ea5e9) - transport publiczny
- **Secondary**: Slate (#64748b) - neutralne, eleganckie
- **Accent**: Teal (#14b8a6) - świeży, nowoczesny
- **Background**: White (#ffffff) / Slate-50 (#f8fafc)
- **Text**: Slate-900 (#0f172a) / Slate-600 (#475569)

---

## 3. Struktura Projektu

```
panel/
├── vite.config.ts
├── tailwind.config.ts
├── package.json
├── .env.local                    # VITE_API_BASE_URL=http://localhost:5000/api/v1
│
└── src/
    ├── main.tsx
    ├── App.tsx
    ├── index.css
    │
    ├── api/                      # Axios client + API endpoints
    │   ├── client.ts             # Interceptory JWT
    │   ├── auth.api.ts
    │   ├── przystanki.api.ts
    │   ├── linie.api.ts
    │   ├── kierowcy.api.ts
    │   ├── pojazdy.api.ts
    │   ├── realizacje.api.ts
    │   ├── grafiki.api.ts
    │   ├── kontrole.api.ts
    │   └── pasazerowie.api.ts
    │
    ├── types/                    # TypeScript interfaces
    │
    ├── hooks/                    # React Query hooks per moduł
    │
    ├── store/                    # Zustand (authStore, uiStore)
    │
    ├── lib/                      # Utils, constants, validators
    │
    ├── components/
    │   ├── ui/                   # shadcn components
    │   ├── layout/               # AppLayout, Sidebar, Header
    │   ├── common/               # DataTable, Pagination, SearchInput
    │   └── forms/                # Formularze per moduł
    │
    ├── features/                 # Strony per moduł
    │   ├── auth/
    │   ├── dashboard/
    │   ├── przystanki/
    │   ├── linie/
    │   ├── kierowcy/
    │   ├── pojazdy/
    │   ├── realizacje/
    │   ├── grafiki/
    │   ├── kontrole/
    │   └── pasazerowie/
    │
    └── routes/
```

---

## 4. Układ Aplikacji

```
+------------------------------------------------------------------+
|  HEADER: Logo | Breadcrumbs                    [User Menu]       |
+--------+---------------------------------------------------------+
|        |                                                         |
| SIDEBAR|   MAIN CONTENT                                          |
| (w-64) |   - Page title + Actions                                |
|        |   - DataTable / Forms                                   |
| [Nav]  |   - Pagination                                          |
|        |                                                         |
+--------+---------------------------------------------------------+
```

**Nawigacja Sidebar:**
- Dashboard
- Sieć komunikacyjna: Przystanki, Linie
- Flota: Pojazdy, Kierowcy
- Operacje: Realizacje kursów, Grafiki pracy
- Kontrole
- Pasażerowie

---

## 5. Moduły do Implementacji

| Moduł | Endpointy API | Funkcjonalności |
|-------|---------------|-----------------|
| **Auth** | POST /auth/login, /auth/refresh | Login, token refresh, logout |
| **Dashboard** | GET z każdego modułu | Statystyki, ostatnia aktywność |
| **Przystanki** | GET/POST /przystanki | Lista, dodawanie, filtrowanie |
| **Linie** | GET/POST /linie | Lista, dodawanie |
| **Kierowcy** | GET/POST /kierowcy, /{id}/uprawnienia, /{id}/badania | CRUD + sub-zasoby |
| **Pojazdy** | GET/POST /pojazdy, /{id}/przeglady | CRUD + przeglądy |
| **Realizacje** | POST /realizacje, /{id}/przypisz | Tworzenie, przypisywanie |
| **Grafiki** | POST /grafiki | Dodawanie wpisów |
| **Kontrole** | POST /kontrole, /{id}/mandaty | Kontrole + mandaty (rola Kontroler) |
| **Pasażerowie** | POST /pasazerowie, /{id}/bilety | Tworzenie + bilety |

---

## 6. Kolejność Implementacji

### Faza 1: Setup projektu
1. `npm create vite@latest . -- --template react-ts`
2. Instalacja zależności (tanstack-query, zustand, axios, tailwind, shadcn)
3. Konfiguracja Tailwind + CSS variables (paleta kolorów)
4. Inicjalizacja shadcn/ui
5. Struktura folderów

### Faza 2: Auth + Layout
1. `src/api/client.ts` - Axios z interceptorami JWT
2. `src/store/authStore.ts` - Zustand persist
3. `src/features/auth/LoginPage.tsx`
4. `src/components/layout/` - AppLayout, Sidebar, Header
5. `src/routes/index.tsx` - ProtectedRoute + routing

### Faza 3: Komponenty wspólne
1. `DataTable.tsx` - generyczna tabela z sortowaniem
2. `Pagination.tsx`
3. `SearchInput.tsx` - z debounce
4. `StatusBadge.tsx`
5. `ConfirmDialog.tsx`

### Faza 4: Dashboard
1. StatCard - liczniki zasobów
2. QuickActions - szybkie akcje

### Faza 5-12: Moduły CRUD
Dla każdego modułu:
1. Typy TypeScript
2. API functions
3. React Query hooks
4. Strona listy z DataTable
5. Dialog/formularz dodawania

Kolejność: Przystanki → Linie → Pojazdy → Kierowcy → Realizacje → Grafiki → Kontrole → Pasażerowie

### Faza 13: Polish
1. Responsywność (mobile sidebar)
2. Error handling
3. Loading states

---

## 7. Kluczowe Pliki do Utworzenia

| Plik | Opis |
|------|------|
| `src/api/client.ts` | Axios + interceptory (token, refresh 401) |
| `src/store/authStore.ts` | Zustand auth state + persist |
| `src/components/layout/AppLayout.tsx` | Główny layout (sidebar + header + content) |
| `src/components/layout/Sidebar.tsx` | Nawigacja z grupami |
| `src/components/common/DataTable.tsx` | Generyczna tabela |
| `src/features/auth/LoginPage.tsx` | Strona logowania |
| `src/features/auth/ProtectedRoute.tsx` | Guard dla tras chronionych |
| `src/routes/index.tsx` | Definicje wszystkich tras |

---

## 8. Weryfikacja

Po implementacji:

```bash
# 1. Uruchom backend
cd backend && docker compose up -d && dotnet run --project src/Transit.Api

# 2. Uruchom panel
cd panel && npm run dev

# 3. Testy manualne:
#    - Logowanie (admin@transit.local / Admin123!)
#    - Nawigacja po modułach
#    - Dodawanie zasobów
#    - Paginacja i filtrowanie
```

---

## 9. Uwagi Implementacyjne

- **CORS**: Backend już skonfigurowany na `*` w dev
- **JWT**: Access token 60 min, refresh 30 dni - implementuj auto-refresh
- **Paginacja**: Wszystkie listy zwracają `{ Items, PageNumber, TotalPages, TotalCount }`
- **Role**: Admin (pełny dostęp), Dyspozytor (flota), Kontroler (kontrole)
- **Walidacja**: Backend zwraca 400 z `ValidationProblemDetails` - wyświetl błędy przy polach
