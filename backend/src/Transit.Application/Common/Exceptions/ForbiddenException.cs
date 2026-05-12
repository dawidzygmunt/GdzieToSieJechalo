namespace Transit.Application.Common.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Brak uprawnień do wykonania tej operacji.") { }
    public ForbiddenException(string message) : base(message) { }
}
