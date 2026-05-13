import { apiClient } from './client';
import type { DodajGrafikDto } from '@/types/grafik.types';

export const grafikiApi = {
  create: async (data: DodajGrafikDto): Promise<number> => {
    const response = await apiClient.post<number>('/grafiki', data);
    return response.data;
  },
};
