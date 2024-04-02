using LangLang.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LangLang.Model;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //TODO: Load data
            new Director("Nadja", "Zoric", "nadjazoric@gmail.com", "PatrikZvezdasti011", Gender.Female, "1234567890123");
            
            User.LoadUsersFromJson();
            
            new MainWindow().Show();
            Exit += App_Exit;
        }
        
        private void App_Exit(object sender, ExitEventArgs e)
        {
            //TODO: Save data
            User.Users.Remove(0); // Remove director
            User.WriteUsersToJson();
        }
    }
}
