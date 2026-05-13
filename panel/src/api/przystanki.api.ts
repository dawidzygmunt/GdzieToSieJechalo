import { apiClient } from './client';
import type { PaginatedResponse } from '@/types/api.types';
import type { PrzystanekListDto, UtworzPrzystanekDto, PrzystankiQueryParams } from '@/types/przystanek.types';

export const przystankiApi = {
  getList: async (params: PrzystankiQueryParams): Promise<PaginatedResponse<PrzystanekListDto>> => {
    const response = await apiClient.get<PaginatedResponse<PrzystanekListDto>>('/przystanki', { params });
    return response.data;
  },

  create: async (data: UtworzPrzystanekDto): Promise<number> => {
    const response = await apiClient.post<number>('/przystanki', data);
    return response.data;
  },
};
