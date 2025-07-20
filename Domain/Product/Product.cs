using Domain.Product.Enums;
using Domain.Product.Events;
using Domain.Product.ValueObjects;

public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public ProductImage? Image { get; private set; }
    public Money Price { get; private set; } = default!;

    public Guid BrandId { get; private set; }
    public Guid CategoryId { get; private set; }

    public int StockQuantity { get; private set; }
    public ProductStatus Status { get; private set; } = ProductStatus.Active;

    private Product() { }

    public static Product Create(string name, string? description, ProductImage? image, Money price, Guid brandId, Guid categoryId, int initialStock = 0)
    {
        ValidateName(name);
        ValidatePrice(price);

        var defaultImage = ProductImage.Of("https://default/image.png");
        var finalImage = image ?? defaultImage;

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Image = finalImage,
            Price = price,
            BrandId = brandId,
            CategoryId = categoryId,
            StockQuantity = initialStock,
            Status = ProductStatus.Active
        };

        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name, product.Price.Value));

        return product;
    }

    public void ChangeName(string name)
    {
        ValidateName(name);
        if (Name != name)
        {
            Name = name;
            //AddDomainEvent(new ProductNameChangedEvent(Id, name));
        }
    }

    public void ChangePrice(Money newPrice)
    {
        ValidatePrice(newPrice);

        if (Price != newPrice)
        {
            Price = newPrice;
            AddDomainEvent(new ProductPriceChangedEvent(Id, Price.Value));
        }
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Stock increment must be positive.");

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Stock decrement must be positive.");

        if (StockQuantity < quantity)
            throw new InvalidOperationException("Insufficient stock.");

        StockQuantity -= quantity;

        if (StockQuantity < 10)
        {
            AddDomainEvent(new ProductStockLowEvent(Id, StockQuantity));
        }
    }

    public void ChangeStatus(ProductStatus status)
    {
        if (status == ProductStatus.Passive && StockQuantity > 0)
            throw new InvalidOperationException("Cannot deactivate product while stock exists.");

        Status = status;
    }

    private static void ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
    }

    private static void ValidatePrice(Money price)
    {
        if (price.Value <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive.");
    }
}
