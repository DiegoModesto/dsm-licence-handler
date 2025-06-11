using Web.API.Extensions;

namespace Web.API;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddEndpoints(typeof(DependencyInjection).Assembly);
        
        services.AddControllers();

        return services;
    }
}
