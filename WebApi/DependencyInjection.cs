using Carter;
using WebApi.Common;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddProblemDetails();
        services.AddCarter();
        return services;
    }
}
