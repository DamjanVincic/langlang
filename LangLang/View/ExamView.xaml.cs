using LangLang.ViewModel;
using System.Windows;
using LangLang.Model;
namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for ExamView.xaml
    /// </summary>
    public partial class ExamView : Window
    {
        public ExamView(Teacher teacher = null)
        {
            DataContext = new ExamListingViewModel(teacher);
            InitializeComponent();
        }
    }
}
