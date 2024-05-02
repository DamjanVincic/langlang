using System.Windows;
using LangLang.ViewModels.CourseViewModels;

namespace LangLang.Views.CourseViews
{
    /// <summary>
    /// Interaction logic for AddCourseView.xaml
    /// </summary>
    public partial class AddCourseView : Window
    {
        public AddCourseView()
        {
            InitializeComponent();
            DataContext = new AddCourseViewModel();
        }
    }
}