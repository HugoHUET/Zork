using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Extensions
{
    public static class DataAccessLayerServiceExtensions
    {
        public static IServiceCollection AddDataAccessLayerService(this IServiceCollection services)
        {
            services.AddDbContext<ZorkDbContext>(options =>
            {
                options.UseSqlServer("Server=localhost;Database=ZorkDb;Trusted_Connection=False;User ID=sa;Password=Root123@;",
                opt => opt.MigrationsAssembly("DataAccessLayer"));
            });

            services.AddTransient<ZorkDbContext>();

            return services;
        }
    }
}
