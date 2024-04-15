using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views
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
