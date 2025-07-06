using Domain.Common;

namespace Domain.Product.ValueObjects;

public class Money : ValueObject
{
    public decimal Value { get; }

    public Money(decimal value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
