using System.Windows;
using LangLang.ViewModels.ExamViewModels;

namespace LangLang.Views.ExamViews
{
    /// <summary>
    /// Interaction logic for ExamView.xaml
    /// </summary>
    public partial class ExamView : Window
    {
        public ExamView()
        {
            DataContext = new ExamListingViewModel();
            InitializeComponent();
        }
    }
}
