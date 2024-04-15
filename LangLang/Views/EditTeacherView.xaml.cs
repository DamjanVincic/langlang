using System.Windows;
using LangLang.Models;
using LangLang.ViewModels;

namespace LangLang.Views
{
    /// <summary>
    /// Interaction logic for EditTeacherView.xaml
    /// </summary>
    public partial class EditTeacherView : Window
    {
        public EditTeacherView(Teacher teacher)
        {
            DataContext = new EditTeacherViewModel(teacher, this);
            InitializeComponent();
        }
    }
}
