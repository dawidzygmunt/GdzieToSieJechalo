import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { pojazdyApi } from '@/api/pojazdy.api';
import type { PojazdyQueryParams, UtworzPojazdDto, DodajPrzegladDto } from '@/types/pojazd.types';

export function usePojazdy(params: PojazdyQueryParams) {
  return useQuery({
    queryKey: ['pojazdy', params],
    queryFn: () => pojazdyApi.getList(params),
  });
}

export function useCreatePojazd() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UtworzPojazdDto) => pojazdyApi.create(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pojazdy'] });
    },
  });
}

export function useDodajPrzeglad() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: DodajPrzegladDto }) =>
      pojazdyApi.dodajPrzeglad(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['pojazdy'] });
    },
  });
}
