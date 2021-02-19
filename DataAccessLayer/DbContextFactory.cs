using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;

namespace DataAccessLayer
{
    public class DbContextFactory: IDesignTimeDbContextFactory<ZorkDbContext>
    {
        public ZorkDbContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<ZorkDbContext>();

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            dbContextBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                opt => opt.MigrationsAssembly("DataAccessLayer"));
           
            return new ZorkDbContext(dbContextBuilder.Options);
        }
    }
}
