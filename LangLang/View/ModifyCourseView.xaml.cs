using LangLang.Model;
using LangLang.ViewModel;
using System.Windows;

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