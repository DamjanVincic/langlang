using System.Windows;
using LangLang.ViewModel;

namespace LangLang.Views
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
