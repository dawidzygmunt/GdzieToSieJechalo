import type { PaginationParams } from './api.types';

export interface KierowcaListDto {
  id: number;
  imie: string;
  nazwisko: string;
  nrPracownika: string;
  dataZatrudnienia: string;
  aktywny: boolean;
}

export interface UtworzKierowceDto {
  imie: string;
  nazwisko: string;
  nrPracownika: string;
  dataZatrudnienia: string;
}

export interface NadajUprawnienieDto {
  idUprawnienia: number;
  dataUzyskania: string;
  dataWaznosci: string;
}

export interface DodajBadanieDto {
  dataBadania: string;
  dataWaznosci: string;
  wynik: string;
  lekarz: string;
}

export interface KierowcyQueryParams extends PaginationParams {
  szukaj?: string;
  aktywny?: boolean;
}
