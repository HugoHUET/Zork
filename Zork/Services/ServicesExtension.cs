using Microsoft.Extensions.DependencyInjection;

namespace Zork.Services
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            return services;
        }
    }
}
