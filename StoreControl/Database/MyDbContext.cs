using Microsoft.EntityFrameworkCore;

namespace StoreControl.Database
{
    public class MyDbContext : DbContext
    {
        public DbSet<Translation> translation { get; set; }
        public DbSet<Products> products { get; set; }
        public DbSet<Categories> categories { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Customers> customers { get; set;}
        public DbSet<Addresses> addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=localhost;database=storecontrol;user=root;password=;";
            try
            {
                ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

                optionsBuilder.UseMySql("server=localhost;database=storecontrol;user=root;password=;", serverVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}" + ex.Message);
            }
        }
        public static async Task<bool> CheckDatabaseConnection()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    await context.Database.OpenConnectionAsync(); 
                    await context.Database.CloseConnectionAsync(); 
                    return true;
                }
            }
            catch
            {
                return false; 
            }
        }
    }
}
