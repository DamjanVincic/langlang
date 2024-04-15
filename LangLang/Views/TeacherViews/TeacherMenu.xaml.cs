using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views.TeacherViews
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
