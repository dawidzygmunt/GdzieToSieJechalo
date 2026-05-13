import { apiClient } from './client';
import type { ZapiszKontroleDto, WystawMandatDto } from '@/types/kontrola.types';

export const kontroleApi = {
  create: async (data: ZapiszKontroleDto): Promise<number> => {
    const response = await apiClient.post<number>('/kontrole', data);
    return response.data;
  },

  wystawMandat: async (id: number, data: WystawMandatDto): Promise<number> => {
    const response = await apiClient.post<number>(`/kontrole/${id}/mandaty`, data);
    return response.data;
  },
};
