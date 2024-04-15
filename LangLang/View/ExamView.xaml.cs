using LangLang.ViewModel;
using System.Windows;

namespace LangLang.View
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
