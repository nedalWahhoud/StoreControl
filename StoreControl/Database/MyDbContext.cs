using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace StoreControl.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<Translation> translation { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Categories> categories { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                ServerVersion serverVersion = ServerVersion.AutoDetect("server=localhost;database=storecontrol;user=root;password=;");

                optionsBuilder.UseMySql("server=localhost;database=storecontrol;user=root;password=;", serverVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}" + ex.Message);
                Flags.isDatabaseConnected = false;
            }
        }
    }
}
