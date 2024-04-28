using LangLang.Model;
using LangLang.ViewModel;
using System.Windows;

namespace LangLang.View
{
    public partial class EditCourseView : Window
    {
        public EditCourseView(Course? course)
        {
            InitializeComponent();
            DataContext = new EditCourseViewModel(course);
        }
    }
}
