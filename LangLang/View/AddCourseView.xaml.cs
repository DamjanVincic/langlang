using LangLang.Model;
using LangLang.ViewModel;
using System.Windows;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for AddCourseView.xaml
    /// </summary>
    public partial class AddCourseView : Window
    {
        public AddCourseView(/*Course? course = null*/)
        {
            InitializeComponent();
            DataContext = new AddCourseViewModel(/*course, this*/);
        }
    }
}