import { apiClient } from './client';
import type { PaginatedResponse } from '@/types/api.types';
import type { LiniaListDto, UtworzLinieDto, LinieQueryParams } from '@/types/linia.types';

export const linieApi = {
  getList: async (params: LinieQueryParams): Promise<PaginatedResponse<LiniaListDto>> => {
    const response = await apiClient.get<PaginatedResponse<LiniaListDto>>('/linie', { params });
    return response.data;
  },

  create: async (data: UtworzLinieDto): Promise<number> => {
    const response = await apiClient.post<number>('/linie', data);
    return response.data;
  },
};
