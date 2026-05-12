namespace Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;

public record PolaczenieDto(
    string NumerLinii,
    string TypLinii,
    string Kierunek,
    string NazwaPrzystankuZ,
    string NazwaPrzystankuDo,
    TimeOnly GodzinaOdjazdu,
    TimeOnly GodzinaPrzyjazdu,
    int CzasTrwaniaMin,
    int LiczbaPrzystankowPosrednich
);
