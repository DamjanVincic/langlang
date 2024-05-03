using System.Windows;
using LangLang.ViewModels.StudentViewModels;

namespace LangLang.Views.StudentViews
{
    /// <summary>
    /// Interaction logic for AppliedExamView.xaml
    /// </summary>
    public partial class AppliedExamView : Window
    {
        public AppliedExamView()
        {
            InitializeComponent();
            DataContext = new AppliedExamListingViewModel();
        }
    }
}
