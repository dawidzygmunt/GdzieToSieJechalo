import { apiClient } from './client';
import type { PaginatedResponse } from '@/types/api.types';
import type { PojazdListDto, UtworzPojazdDto, DodajPrzegladDto, PojazdyQueryParams } from '@/types/pojazd.types';

export const pojazdyApi = {
  getList: async (params: PojazdyQueryParams): Promise<PaginatedResponse<PojazdListDto>> => {
    const response = await apiClient.get<PaginatedResponse<PojazdListDto>>('/pojazdy', { params });
    return response.data;
  },

  create: async (data: UtworzPojazdDto): Promise<number> => {
    const response = await apiClient.post<number>('/pojazdy', data);
    return response.data;
  },

  dodajPrzeglad: async (id: number, data: DodajPrzegladDto): Promise<number> => {
    const response = await apiClient.post<number>(`/pojazdy/${id}/przeglady`, data);
    return response.data;
  },
};
