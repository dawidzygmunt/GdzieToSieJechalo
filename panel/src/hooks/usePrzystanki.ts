import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { przystankiApi } from '@/api/przystanki.api';
import type { PrzystankiQueryParams, UtworzPrzystanekDto } from '@/types/przystanek.types';

export function usePrzystanki(params: PrzystankiQueryParams) {
  return useQuery({
    queryKey: ['przystanki', params],
    queryFn: () => przystankiApi.getList(params),
  });
}

export function useCreatePrzystanek() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UtworzPrzystanekDto) => przystankiApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['przystanki'] });
    },
  });
}
