import type { PaginationParams } from './api.types';

export interface PrzystanekListDto {
  id: number;
  nazwa: string;
  ulica: string | null;
  typ: string;
  wiata: boolean;
  dzielnica: string;
  aktywny: boolean;
}

export interface UtworzPrzystanekDto {
  idDzielnicy: number;
  nazwa: string;
  ulica?: string;
  typ: string;
  wiata: boolean;
}

export interface PrzystankiQueryParams extends PaginationParams {
  idDzielnicy?: number;
  szukaj?: string;
}
