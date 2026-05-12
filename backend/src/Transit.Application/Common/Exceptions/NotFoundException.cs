namespace Transit.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Nie znaleziono '{name}' o kluczu '{key}'.") { }

    public NotFoundException(string message) : base(message) { }
}
