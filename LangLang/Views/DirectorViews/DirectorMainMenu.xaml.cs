using LangLang.ViewModels.TeacherViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LangLang.ViewModels.DirectorViewModels;


namespace LangLang.Views.DirectorViews
{
    /// <summary>
    /// Interaction logic for DirectorMainMenu.xaml
    /// </summary>
    public partial class DirectorMainMenu : Window
    {
        public DirectorMainMenu()
        {
            InitializeComponent();
            DataContext = new DirectorViewModel(this);
        }
    }
}
