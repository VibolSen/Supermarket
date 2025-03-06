using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace Supermarket.Data
{
    public class SupermarketContextFactory : IDesignTimeDbContextFactory<SupermarketContext>
    {
        public SupermarketContext CreateDbContext(string[] args)
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SupermarketContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyDatabaseConnection"));

            return new SupermarketContext(optionsBuilder.Options);
        }
    }
}