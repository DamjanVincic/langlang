using LangLang.ViewModel;
using System.Windows;
using LangLang.Models;

namespace LangLang.View
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