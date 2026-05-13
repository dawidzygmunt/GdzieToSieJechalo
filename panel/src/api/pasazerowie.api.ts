import { apiClient } from './client';
import type { UtworzPasazeraDto, WystawBiletDto } from '@/types/pasazer.types';

export const pasazerowieApi = {
  create: async (data: UtworzPasazeraDto): Promise<number> => {
    const response = await apiClient.post<number>('/pasazerowie', data);
    return response.data;
  },

  wystawBilet: async (id: number, data: WystawBiletDto): Promise<number> => {
    const response = await apiClient.post<number>(`/pasazerowie/${id}/bilety`, data);
    return response.data;
  },
};
