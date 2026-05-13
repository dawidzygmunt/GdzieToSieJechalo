export type TypDnia = 'ROB' | 'SOB' | 'SWI';

export interface PolaczenieDto {
  numerLinii: string;
  typLinii: string;
  kierunek: string;
  nazwaPrzystankuZ: string;
  nazwaPrzystankuDo: string;
  godzinaOdjazdu: string;
  godzinaPrzyjazdu: string;
  czasTrwaniaMin: number;
  liczbaPrzystankowPosrednich: number;
}

export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface PrzystanekListDto {
  id: number;
  nazwa: string;
  ulica: string;
  typ: string;
  wiata: boolean;
  dzielnica: string;
  aktywny: boolean;
}

export interface LiniaListDto {
  id: number;
  numerLinii: string;
  typLinii: string;
  opis: string | null;
  aktywna: boolean;
  liczbaWariantow: number;
}

export interface KursDto {
  nrKursu: number;
  przystanki: { nazwaPrzystanku: string; godzinaOdjazdu: string }[];
}

export interface WariantRozkladDto {
  nazwaWariantu: string;
  kierunek: string;
  kursy: KursDto[];
}

export interface RozkladLiniiDto {
  numerLinii: string;
  kodTypuDnia: TypDnia;
  warianty: WariantRozkladDto[];
}
