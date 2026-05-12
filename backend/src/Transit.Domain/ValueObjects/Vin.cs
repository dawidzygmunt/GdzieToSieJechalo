using System.Text.RegularExpressions;
using Transit.Domain.Exceptions;

namespace Transit.Domain.ValueObjects;

public sealed class Vin : IEquatable<Vin>
{
    private static readonly Regex Pattern = new(@"^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.Compiled);

    public string Value { get; }

    public Vin(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Pattern.IsMatch(value.ToUpperInvariant()))
            throw new DomainException($"'{value}' nie jest prawidłowym numerem VIN.");
        Value = value.ToUpperInvariant();
    }

    public static implicit operator string(Vin vin) => vin.Value;

    public bool Equals(Vin? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Vin v && Equals(v);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
