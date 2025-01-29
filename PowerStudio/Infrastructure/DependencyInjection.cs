using Azure.ResourceManager;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PowerStudio.Interfaces;
using PowerStudio.Models.Settings;
using PowerStudio.Services;
using PowerStudio.ViewModels;
using System;
using System.IO;

namespace PowerStudio.Infrastructure
{
    public class DependencyInjection
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static void Build()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IAuthenticationService, AuthenticationService>(x => new AuthenticationService(LoadAzureAdSettings()));
            services.AddSingleton<AzureService>();
            services.AddSingleton(serviceProvider =>
            {
                var authenticationService = serviceProvider.GetService<IAuthenticationService>();
                var msalTokenCredential = new MsalTokenCredential(authenticationService);
                return new ArmClient(msalTokenCredential);
            });


            // Register your forms

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MainWindowControl>();
        }

        private static AzureAdSettings LoadAzureAdSettings()
        {
            var json = File.ReadAllText("appsettings.json");
            var settings = JsonConvert.DeserializeObject<AzureAdSettings>(json);
            return settings;
        }
    }
}
