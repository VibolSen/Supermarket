using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Supermarket.Services;
using Supermarket.Models;
using Supermarket.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Add this using statement
using System.IO;
using System.Linq;

namespace Supermarket
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            // Configure Dependency Injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Get the configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Ensure correct base path
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Register the Database Context using the connection string from appsettings.json
            services.AddDbContext<SupermarketContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyDatabaseConnection")); // Use YOUR connection string name!
            });

            services.AddSingleton<InventoryService>();
            services.AddTransient<MainWindow>();
        }

        public new static App Current => (App)Application.Current; // Corrected cast
        public IServiceProvider ServiceProvider => _serviceProvider;  // Publicly expose the service provider.

    }
}