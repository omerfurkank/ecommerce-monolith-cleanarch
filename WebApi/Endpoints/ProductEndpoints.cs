using Application.Products.Commands;
using Carter;
using MediatR;

namespace WebApi.Endpoints;

public class ProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/products", async (CreateProductCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/products/{id}", new { id });
        });
    }
}
