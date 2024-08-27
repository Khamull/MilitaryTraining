using BLL.Services;
using DAL;
using DAL.Context;
using DAL.Models;
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using UI.Helpers;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            // Import data from JSON file
            // Start the main window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            // Register your services
            services.AddSingleton<IDataRepository, InMemoryDataRepository>();
            services.AddTransient<MovementService>();
            services.AddTransient<UnitServices>();

            // Register your main window
            services.AddTransient<MainWindow>();
        }

    }
}
