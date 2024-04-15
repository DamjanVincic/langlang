using System.Windows;
using LangLang.Models;
using LangLang.ViewModel;

namespace LangLang.Views
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