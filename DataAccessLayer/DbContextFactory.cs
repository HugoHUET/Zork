using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccessLayer
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ZorkDbContext>
    {
        public ZorkDbContext CreateDbContext(string[] args)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<ZorkDbContext>();

            dbContextBuilder.UseSqlServer("Server=localhost;Database=ZorkDb;Trusted_Connection=False;User ID=sa;Password=reallyStrongPwd123;",
                opt => opt.MigrationsAssembly("DataAccessLayer"));

            return new ZorkDbContext(dbContextBuilder.Options);
        }
    }
}
