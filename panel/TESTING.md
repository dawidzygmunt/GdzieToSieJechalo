# Testowanie aplikacji Panel

## Konfiguracja testów

Projekt używa następujących narzędzi testowych:
- **Vitest** - framework do testów jednostkowych i integracyjnych
- **React Testing Library** - do testowania komponentów React
- **MSW (Mock Service Worker)** - do mockowania API requests
- **@testing-library/user-event** - do symulacji interakcji użytkownika

## Uruchamianie testów

```bash
# Uruchom wszystkie testy
npm test

# Uruchom testy w trybie watch
npm test -- --watch

# Uruchom testy z UI
npm run test:ui

# Uruchom testy z coverage
npm run test:coverage
```

## Struktura testów

### Testy jednostkowe (Unit Tests)
Lokalizacja: `src/**/*.test.tsx`

Testują pojedyncze komponenty w izolacji z zmockowanymi zależnościami.

Przykład: `src/features/kierowcy/AddKierowcaDialog.test.tsx`
- Renderowanie komponentu
- Walidacja formularzy
- Obsługa stanów (loading, error)
- Interakcje użytkownika

### Testy integracyjne (Integration Tests)
Lokalizacja: `src/**/*.integration.test.tsx`

Testują przepływ danych między komponentami, hookami i API z użyciem MSW.

Przykład: `src/features/kierowcy/AddKierowcaDialog.integration.test.tsx`
- Pełny przepływ dodawania kierowcy
- Rzeczywiste wywołania API (zmockowane przez MSW)
- Obsługa błędów API
- Walidacja formatów danych

### Testy API
Lokalizacja: `src/api/**/*.test.ts`

Testują funkcje API i format wysyłanych danych.

Przykład: `src/api/kierowcy.api.test.ts`
- Poprawność parametrów requests
- Format danych (camelCase, daty w ISO)
- Obsługa błędów

## MSW (Mock Service Worker)

MSW pozwala na mockowanie requestów HTTP na poziomie sieciowym, dzięki czemu testy są bardziej zbliżone do rzeczywistości.

### Konfiguracja
- `src/test/mocks/handlers.ts` - definicje mock handlers
- `src/test/mocks/server.ts` - konfiguracja serwera MSW
- `src/test/setup.ts` - inicjalizacja MSW przed testami

### Dodawanie nowych handlerów

Przykład dodania nowego handlera dla API:

\`\`\`typescript
// src/test/mocks/handlers.ts
export const handlers = [
  http.post(\`\${API_BASE_URL}/nowy-endpoint\`, async ({ request }) => {
    const body = await request.json();
    // Walidacja i zwrócenie odpowiedzi
    return HttpResponse.json({ success: true }, { status: 201 });
  }),
];
\`\`\`

## Pisanie testów

### Testowanie formularzy

\`\`\`typescript
import { render, screen, waitFor } from '@/test/utils';
import userEvent from '@testing-library/user-event';

it('should submit form with valid data', async () => {
  const user = userEvent.setup();
  render(<MyForm />);

  await user.type(screen.getByLabelText(/Name/i), 'John');
  await user.click(screen.getByRole('button', { name: /Submit/i }));

  await waitFor(() => {
    expect(screen.queryByText(/Success/i)).toBeInTheDocument();
  });
});
\`\`\`

### Mockowanie hooków

\`\`\`typescript
import { vi } from 'vitest';
import * as useMyHook from '@/hooks/useMyHook';

vi.mock('@/hooks/useMyHook', () => ({
  useMyHook: vi.fn(),
}));

// W teście:
vi.mocked(useMyHook.useMyHook).mockReturnValue({
  data: null,
  isLoading: false,
  // ...
});
\`\`\`

## Pokrycie testami

Obecne pokrycie:
- ✅ Komponenty dialogów (AddKierowcaDialog, AddLiniaDialog, etc.)
- ✅ API functions (kierowcyApi)
- ✅ Integracja z MSW

Do zrobienia:
- [ ] Testy dla pozostałych dialogów (AddPojazdDialog, AddPrzystanekDialog, AddLiniaDialog)
- [ ] Testy dla stron list (KierowcyListPage, etc.)
- [ ] Testy dla common components (DataTable, Pagination, etc.)
- [ ] Testy dla hooków (useKierowcy, usePagination, etc.)

## Debugowanie testów

### Wyświetlanie aktualnego DOM
\`\`\`typescript
import { screen } from '@/test/utils';

// W teście:
screen.debug(); // Wyświetla cały DOM
screen.debug(screen.getByRole('button')); // Wyświetla konkretny element
\`\`\`

### Logowanie w MSW
Wszystkie requesty do API są logowane w konsoli podczas testów dzięki konfiguracji w `src/api/client.ts`.

## Najlepsze praktyki

1. **Testuj zachowanie, nie implementację** - Testy powinny skupiać się na tym co użytkownik widzi i robi, nie na szczegółach implementacji.

2. **Używaj semantic queries** - Preferuj `getByRole`, `getByLabelText` nad `getByTestId`.

3. **Czekaj na asynchroniczne akcje** - Zawsze używaj `waitFor` dla operacji asynchronicznych.

4. **Cleanup** - Vitest automatycznie czyści po każdym teście dzięki konfiguracji w `src/test/setup.ts`.

5. **Mock tylko gdy trzeba** - Testuj z prawdziwymi komponentami tam gdzie możliwe, mockuj tylko zewnętrzne zależności (API, third-party libs).

## Rozwiązywanie problemów

### "Not implemented: Window.alert()"
Alert jest zmockowany globalnie w `src/test/setup.ts`. Jeśli widzisz ten błąd, upewnij się że setup jest załadowany.

### "Cannot find module"
Sprawdź czy alias `@/` jest poprawnie skonfigurowany w `vitest.config.ts`.

### Testy przechodzą lokalnie ale nie w CI
Upewnij się że wszystkie zależności są zainstalowane w CI i że używasz tej samej wersji Node.js.
