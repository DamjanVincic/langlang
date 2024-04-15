using System.Windows;
using LangLang.ViewModel;

namespace LangLang.Views
{
    /// <summary>
    /// Interaction logic for TeachersView.xaml
    /// </summary>
    public partial class TeachersView : Window
    {
        public TeachersView()
        {
            DataContext = new TeacherListingViewModel(this);

            InitializeComponent();
        }
    }
}
