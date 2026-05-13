import type { PaginationParams } from './api.types';

export interface LiniaListDto {
  id: number;
  numerLinii: string;
  typLinii: string;
  opis: string | null;
  aktywna: boolean;
  liczbaWariantow: number;
}

export interface UtworzLinieDto {
  numerLinii: string;
  typLinii: string;
  opis?: string;
}

export interface LinieQueryParams extends PaginationParams {
  szukaj?: string;
}
