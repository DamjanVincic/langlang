using System;
using System.Linq;
using System.Windows;
using LangLang.Models;
using LangLang.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            
            Director director = new Director("Nadja", "Zoric", "nadjazoric@gmail.com", "PatrikZvezdasti011", Gender.Female, "1234567890123");
            
            IUserRepository userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            if (userRepository.GetAll().All(user => user.Email != director.Email))
                userRepository.Add(director);
            
            Exit += App_Exit;
        }
        
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddSingleton<IUserRepository, UserFileRepository>();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        
        private void App_Exit(object sender, ExitEventArgs e)
        {
            
        }
    }
}
