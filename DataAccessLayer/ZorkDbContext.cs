using System;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class ZorkDbContext: DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Monster> Monsters { get; set; }
        public DbSet<Models.Object> Objects { get; set; }
        public DbSet<Weapon> Weapons { get; set; }


        public ZorkDbContext(DbContextOptions<ZorkDbContext> options): base(options)
        {
        }
        
    }
}
