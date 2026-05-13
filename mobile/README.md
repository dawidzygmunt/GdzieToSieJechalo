# GdzieToSieJechało — mobile

Prosta aplikacja React Native (Expo) — klient dla backendu w `../backend`.

## Funkcje (MVP)

- **Wyszukiwarka połączeń A → B** — wybór dwóch przystanków, daty i czasu; lista najbliższych kursów (`GET /api/v1/polaczenia`).
- **Rozkład linii** — lista linii, po wyborze tabela rozkładu dla wybranego typu dnia (`ROB`/`SOB`/`SWI`).

## Wymagania

- Node.js 20+ i `npm`.
- Uruchomiony backend (`cd ../backend && docker compose up -d && dotnet run --project src/Transit.Api`).
- Do testów na telefonie — aplikacja **Expo Go** (App Store / Google Play).

## Konfiguracja API

Domyślny URL: `http://localhost:5000`. Skopiuj `.env.example` do `.env` i dostosuj:

| Środowisko | `EXPO_PUBLIC_API_URL` |
|---|---|
| Web (`npm run web`) | `http://localhost:5000` |
| Emulator Android | `http://10.0.2.2:5000` |
| Telefon fizyczny | `http://<IP-twojego-PC>:5000` (komputer i telefon w tej samej sieci) |

> Jeśli backend startuje na porcie 5210 (z `launchSettings.json`), dostosuj odpowiednio.

## Uruchomienie

```bash
npm install
npm run web        # przeglądarka — najszybciej do sprawdzenia
# lub
npm run android    # wymaga emulatora Android Studio
npm run ios        # wymaga macOS + Xcode
npm start          # tryb interaktywny: skan QR z Expo Go
```

## Struktura

```
mobile/
├── App.tsx                          # NavigationContainer + Stack
├── src/
│   ├── api/                         # client.ts + DTO types
│   ├── components/                  # PolaczenieCard, DayTypePicker, ErrorView
│   ├── navigation/types.ts          # RootStackParamList
│   └── screens/                     # Home, Polaczenia, StopPicker, Linie, RozkladLinii
```

## Smoke test

1. Backend działa, dane seedu demo są wczytane (admin@transit.local).
2. Home → **Wyszukaj połączenie** → wybierz przystanek startowy → cel → data `2026-05-13`, czas `08:00` → **Szukaj** → lista połączeń linii 180.
3. Home → **Rozkład linii** → kliknij `180` → przełącz dni `ROB / SOB / SWI` → tabela rozkładu.

## Świadomie poza MVP

Brak logowania (endpointy są `[AllowAnonymous]`), brak tablicy odjazdów z przystanku, brak mapy, brak offline. Plan rozbudowy patrz `../backend/plan.md`.
