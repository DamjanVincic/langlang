using LangLang.ViewModels.CourseViewModels;
using System.Windows;

namespace LangLang.Views.CourseViews
{
    /// <summary>
    /// Interaction logic for StartableCoursesView.xaml
    /// </summary>
    public partial class StartableCoursesView : Window
    {
        public StartableCoursesView()
        {
            InitializeComponent();
            DataContext = new StartableCoursesViewModel();
        }
    }
}
