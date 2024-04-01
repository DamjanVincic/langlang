using LangLang.Model;
using LangLang.ViewModel;
using System.Windows;

namespace LangLang.View
{
    public partial class TeacherMenu : Window
    {
        public TeacherMenu(Teacher teacher)
        {
            InitializeComponent();
            DataContext = new TeacherMenuViewModel(teacher);
        }
    }
}
