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
    public static Money operator -(Money a, Money b)
    {
        if (a.Value - b.Value < 0)
            throw new InvalidOperationException("Money cannot be negative.");

        return Of(a.Value - b.Value);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
