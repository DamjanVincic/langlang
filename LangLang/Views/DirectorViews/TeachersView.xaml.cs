using System.Windows;
using LangLang.ViewModels.TeacherViewModels;

namespace LangLang.Views.DirectorViews
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
