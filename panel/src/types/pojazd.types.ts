import type { PaginationParams } from './api.types';

export interface PojazdListDto {
  id: number;
  numerBoczny: string;
  vin: string;
  rokProdukcji: number;
  model: string;
  producent: string;
  aktywny: boolean;
}

export interface UtworzPojazdDto {
  idModelu: number;
  numerBoczny: string;
  vin: string;
  rokProdukcji: number;
  dataZakupu: string;
}

export interface DodajPrzegladDto {
  idTypuPrzegladu: number;
  dataPrzegladu: string;
  wynik: string;
  warsztat: string;
  uwagi?: string;
}

export interface PojazdyQueryParams extends PaginationParams {
  aktywny?: boolean;
}
