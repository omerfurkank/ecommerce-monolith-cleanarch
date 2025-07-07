using Domain.Common;

namespace Domain.Product.ValueObjects;

public class Money : ValueObject
{
    public decimal Value { get; }

    private Money(decimal value)
    {
        Value = value;
    }

    public static Money Of(decimal value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Money value cannot be negative.");
        return new Money(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
