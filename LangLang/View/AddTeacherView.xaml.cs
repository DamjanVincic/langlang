using LangLang.ViewModel;
using System.Windows;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for AddTeacherView.xaml
    /// </summary>
    public partial class AddTeacherView : Window
    {
        public AddTeacherView()
        {
            InitializeComponent();
            DataContext = new AddTeacherViewModel(QualificationsListBox, this);
        }
    }
}
