using Domain.Common;

namespace Domain.Product.Events;

public class ProductStockLowEvent : DomainEvent
{
    public Guid ProductId { get; }
    public int StockQuantity { get; }

    public ProductStockLowEvent(Guid productId, int stockQuantity)
    {
        ProductId = productId;
        StockQuantity = stockQuantity;
    }
}