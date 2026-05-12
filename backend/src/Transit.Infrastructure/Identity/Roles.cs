namespace Transit.Infrastructure.Identity;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Dyspozytor = "Dyspozytor";
    public const string Kontroler = "Kontroler";
    public const string Kierowca = "Kierowca";
    public const string Pasazer = "Pasazer";

    public static readonly string[] All = [Admin, Dyspozytor, Kontroler, Kierowca, Pasazer];
}
