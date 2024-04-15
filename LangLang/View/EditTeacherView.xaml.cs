using System.Windows;
using LangLang.Models;
using LangLang.ViewModel;

namespace LangLang.View
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
