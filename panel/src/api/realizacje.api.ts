import { apiClient } from './client';
import type { UtworzRealizacjeDto, PrzypiszKierowcePojazdDto } from '@/types/realizacja.types';

export const realizacjeApi = {
  create: async (data: UtworzRealizacjeDto): Promise<number> => {
    const response = await apiClient.post<number>('/realizacje', data);
    return response.data;
  },

  przypisz: async (id: number, data: PrzypiszKierowcePojazdDto): Promise<void> => {
    await apiClient.post(`/realizacje/${id}/przypisz`, data);
  },
};
