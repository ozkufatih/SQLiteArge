using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Playground.Data.Contexts;
using Playground.Data.Repositories;
using Playground.Services.AutoMapper;
using Playground.Services.Services;
using Playground.Utils.ConsoleHelper;

namespace Playground.App
{
    public class Program
    {
        public static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            ConfigureServices();

            ConsoleExtensions.ToColorWriteLine("Welcome to Playground", ConsoleColor.Yellow);

            AppMenu appMenu = new();
            appMenu.ShowMainMenu();

            DisposeServices();
        }

        private static void ConfigureServices()
        {
            // Setup DI container (example using built-in DI for simplicity)
            var services = new ServiceCollection();

            // Load configuration from appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //AppSettings.ConnectionString = configuration.GetConnectionString("DefaultConnection");

            // Register dependencies
            services.AddDbContext<DataContext>();
            services.AddTransient<IPortfolioRepository, PortfolioRepository>(); // Register PortfolioRepository
            services.AddTransient<IAssetRepository, AssetRepository>(); // Register AssetRepository
            services.AddTransient<IPortfolioAssetRepository, PortfolioAssetRepository>(); // Register PortfolioAssetRepository
            services.AddTransient<IAssetTypeRepository, AssetTypeRepository>(); // Register AssetTypeRepository
            services.AddTransient<IPortfolioService, PortfolioService>(); // Register PortfolioService
            services.AddTransient<IAssetService, AssetService>(); // Register AssetService
            services.AddTransient<IAssetTypeService, AssetTypeService>(); // Register AssetTypeService

            services.AddAutoMapper(typeof(AutoMapperProfile)); // Register AutoMapper

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }

            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
