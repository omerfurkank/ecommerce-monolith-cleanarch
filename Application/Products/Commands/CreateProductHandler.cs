using Application.Common.Interfaces;
using Domain.Product.ValueObjects;
using FluentValidation;
using MediatR;

namespace Application.Products.Commands;
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required").MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.ImageUrl).MaximumLength(500);
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
        RuleFor(x => x.BrandId).NotEmpty().WithMessage("BrandId is required");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required");
        RuleFor(x => x.InitialStock).GreaterThanOrEqualTo(0).WithMessage("InitialStock cannot be negative");
    }
}
public record CreateProductCommand(
    string Name,
    string? Description,
    string? ImageUrl,
    decimal Price,
    Guid BrandId,
    Guid CategoryId,
    int InitialStock
) : IRequest<Guid>;

public class CreateProductHandler(IApplicationDbContext context)
    : IRequestHandler<CreateProductCommand, Guid>
{

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //var brandExists = await context.Brands.AnyAsync(b => b.Id == request.BrandId, cancellationToken);
        //if (!brandExists)
        //    throw new ArgumentException($"Brand with Id {request.BrandId} not found.");

        //var categoryExists = await context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        //if (!categoryExists)
        //    throw new ArgumentException($"Category with Id {request.CategoryId} not found.");

        Money price = Money.Of(request.Price);
        ProductImage? image = string.IsNullOrWhiteSpace(request.ImageUrl)
            ? null : ProductImage.Of(request.ImageUrl);

        var product = Product.Create(
             request.Name,
             request.Description,
             image,
             price,
             request.BrandId,
             request.CategoryId,
             request.InitialStock
         );

        context.Products.Add(product);

        await context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
