using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.ComplexProperty(p => p.Price, priceBuilder =>
        {
            priceBuilder.Property(pv => pv.Value).IsRequired().HasMaxLength(50);
        });

        builder.OwnsOne(p => p.Image, imageBuilder =>
        {
            imageBuilder.Property(pi => pi.Url)
                .HasColumnName("ImageUrl")
                .IsRequired();
        });

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.BrandId)
            .IsRequired();

        builder.Property(p => p.CategoryId)
            .IsRequired();
    }
}
