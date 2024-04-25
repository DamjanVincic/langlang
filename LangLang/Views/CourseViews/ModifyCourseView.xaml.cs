using System.Windows;
using LangLang.Models;
using LangLang.ViewModels.CourseViewModels;

namespace LangLang.Views.CourseViews
{
    /// <summary>
    /// Interaction logic for AddCourseView.xaml
    /// </summary>
    public partial class ModifyCourseView : Window
    {
        public ModifyCourseView(Course? course = null)
        {
            InitializeComponent();
            DataContext = new ModifyCourseViewModel(course, this);
        }
    }
}