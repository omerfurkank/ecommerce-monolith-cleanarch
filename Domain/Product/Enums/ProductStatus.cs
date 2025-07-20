using Domain.Common.Interfaces;

namespace Domain.Product.Enums;

[SoftDeletableStatus]
public enum ProductStatus : byte
{
    Active = 1,
    Passive = 2,
    Deleted = 3
}

[AttributeUsage(AttributeTargets.Enum)]
public sealed class SoftDeletableStatusAttribute : Attribute { }
