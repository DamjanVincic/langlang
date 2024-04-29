using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Director director = new Director("Nadja", "Zoric", "nadjazoric@gmail.com", "PatrikZvezdasti011", Gender.Female, "1234567890123");
            
            IUserRepository userRepository = new UserFileRepository();
            if (userRepository.GetAll().All(user => user.Email != director.Email))
                userRepository.Add(director);
            
            new MainWindow().Show();
            Exit += App_Exit;
        }
        
        private void App_Exit(object sender, ExitEventArgs e)
        {
            
        }
    }
}
