using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ZorkDbContext>
    {
        public ZorkDbContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<ZorkDbContext>();

            dbContextBuilder.UseSqlServer($"Server={args[0]};Database={args[1]};Trusted_Connection=False;User ID={args[2]};Password={args[3]};",
                opt => opt.MigrationsAssembly("DataAccessLayer"));

            return new ZorkDbContext(dbContextBuilder.Options);
        }
    }
}
