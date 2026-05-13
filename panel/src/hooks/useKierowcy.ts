import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { kierowcyApi } from '@/api/kierowcy.api';
import type { KierowcyQueryParams, UtworzKierowceDto, NadajUprawnienieDto, DodajBadanieDto } from '@/types/kierowca.types';

export function useKierowcy(params: KierowcyQueryParams) {
  return useQuery({
    queryKey: ['kierowcy', params],
    queryFn: () => kierowcyApi.getList(params),
  });
}

export function useCreateKierowca() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UtworzKierowceDto) => kierowcyApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['kierowcy'] });
    },
  });
}

export function useNadajUprawnienie() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: NadajUprawnienieDto }) =>
      kierowcyApi.nadajUprawnienie(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['kierowcy'] });
    },
  });
}

export function useDodajBadanie() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: DodajBadanieDto }) =>
      kierowcyApi.dodajBadanie(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['kierowcy'] });
    },
  });
}
