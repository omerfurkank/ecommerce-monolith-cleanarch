using Domain.Common;

namespace Domain.Product.Events;

public class ProductPriceChangedEvent : DomainEvent
{
    public Guid ProductId { get; }
    public decimal NewPrice { get; }

    public ProductPriceChangedEvent(Guid productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }
}
