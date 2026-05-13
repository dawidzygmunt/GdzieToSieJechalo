export interface UtworzPasazeraDto {
  Imie: string;
  Nazwisko: string;
  Pesel?: string;
  Email?: string;
}

export interface WystawBiletDto {
  IdKategorii: number;
  DataOd: string;
  DataDo: string;
  Cena: number;
}
