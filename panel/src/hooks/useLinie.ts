import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { linieApi } from '@/api/linie.api';
import type { LinieQueryParams, UtworzLinieDto } from '@/types/linia.types';

export function useLinie(params: LinieQueryParams) {
  return useQuery({
    queryKey: ['linie', params],
    queryFn: () => linieApi.getList(params),
  });
}

export function useCreateLinia() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UtworzLinieDto) => linieApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['linie'] });
    },
  });
}
