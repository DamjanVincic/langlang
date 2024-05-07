using LangLang.ViewModels.CourseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.Views.CourseViews
{
    /// <summary>
    /// Interaction logic for ActiveCoursesView.xaml
    /// </summary>
    public partial class ActiveCoursesView : Window
    {
        public ActiveCoursesView()
        {
            InitializeComponent();
            DataContext = new ActiveCoursesViewModel();
        }
    }
}
