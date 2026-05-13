export interface ZapiszKontroleDto {
  IdKontrolera: number;
  IdRealizacji: number;
  DataGodzina: string;
  Wynik: string;
}

export interface WystawMandatDto {
  Kwota: number;
  Powod: string;
  IdPasazera?: number;
  NrDokumentu?: string;
}
