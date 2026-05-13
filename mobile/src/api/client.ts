const HOST = process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:5000';
export const BASE_URL = `${HOST}/api/v1`;

type Params = Record<string, string | number | boolean | undefined | null>;

function buildQuery(params?: Params): string {
  if (!params) return '';
  const entries = Object.entries(params).filter(
    ([, v]) => v !== undefined && v !== null && v !== ''
  );
  if (entries.length === 0) return '';
  const qs = new URLSearchParams(entries.map(([k, v]) => [k, String(v)])).toString();
  return `?${qs}`;
}

export async function apiGet<T>(path: string, params?: Params): Promise<T> {
  const url = `${BASE_URL}${path}${buildQuery(params)}`;
  const res = await fetch(url);
  if (!res.ok) {
    let detail = res.statusText;
    try {
      const body = await res.text();
      if (body) detail = body;
    } catch {}
    throw new Error(`HTTP ${res.status}: ${detail}`);
  }
  return (await res.json()) as T;
}
