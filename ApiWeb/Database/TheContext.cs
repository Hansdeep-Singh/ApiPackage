using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiWeb.Database
{
    public class TheContext : DbContext
    {
        public TheContext() { }
        public TheContext(DbContextOptions<TheContext> options)
           : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<TheToken> TheTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                //IConfigurationRoot configuration = new ConfigurationBuilder()
                //   .SetBasePath(Directory.GetCurrentDirectory())
                //   .AddJsonFile("appsettings.json")
                //   .Build();
                //var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
                //optionsBuilder.UseSqlServer(connectionString);
            }
        }



    }
}
