import { PrzystanekListDto } from '@/api/types';

export type RootStackParamList = {
  Home: undefined;
  Polaczenia: undefined;
  StopPicker: {
    title?: string;
    onSelect: (stop: PrzystanekListDto) => void;
  };
  Linie: undefined;
  RozkladLinii: {
    id: number;
    numerLinii: string;
  };
};
