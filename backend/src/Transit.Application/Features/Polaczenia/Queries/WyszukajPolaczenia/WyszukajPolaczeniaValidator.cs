using FluentValidation;

namespace Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;

public class WyszukajPolaczeniaValidator : AbstractValidator<WyszukajPolaczeniaQuery>
{
    public WyszukajPolaczeniaValidator()
    {
        RuleFor(x => x.PrzystanekZ).GreaterThan(0).WithMessage("ID przystanku 'z' musi być większe od 0.");
        RuleFor(x => x.PrzystanekDo).GreaterThan(0).WithMessage("ID przystanku 'do' musi być większe od 0.");
        RuleFor(x => x).Must(x => x.PrzystanekZ != x.PrzystanekDo).WithMessage("Przystanek początkowy i końcowy muszą być różne.");
        RuleFor(x => x.MaxWynikow).InclusiveBetween(1, 50).WithMessage("Maksymalna liczba wyników musi być między 1 a 50.");
    }
}
