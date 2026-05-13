import { apiClient } from './client';
import type { PaginatedResponse } from '@/types/api.types';
import type {
  KierowcaListDto,
  UtworzKierowceDto,
  NadajUprawnienieDto,
  DodajBadanieDto,
  KierowcyQueryParams,
} from '@/types/kierowca.types';

export const kierowcyApi = {
  getList: async (params: KierowcyQueryParams): Promise<PaginatedResponse<KierowcaListDto>> => {
    const response = await apiClient.get<PaginatedResponse<KierowcaListDto>>('/kierowcy', { params });
    return response.data;
  },

  create: async (data: UtworzKierowceDto): Promise<number> => {
    const response = await apiClient.post<number>('/kierowcy', data);
    return response.data;
  },

  nadajUprawnienie: async (id: number, data: NadajUprawnienieDto): Promise<number> => {
    const response = await apiClient.post<number>(`/kierowcy/${id}/uprawnienia`, data);
    return response.data;
  },

  dodajBadanie: async (id: number, data: DodajBadanieDto): Promise<number> => {
    const response = await apiClient.post<number>(`/kierowcy/${id}/badania`, data);
    return response.data;
  },
};
