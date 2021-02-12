using System;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class ZorkDbContext: DbContext
    {
        public ZorkDbContext(DbContextOptions<ZorkDbContext> options): base(options)
        {
        }
    }
}
