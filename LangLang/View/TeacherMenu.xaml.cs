using LangLang.ViewModel;
using System.Windows;

namespace LangLang.View
{
    public partial class TeacherMenu : Window
    {
        public TeacherMenu()
        {
            InitializeComponent();
            DataContext = new TeacherMenuViewModel(this);
        }
    }
}
