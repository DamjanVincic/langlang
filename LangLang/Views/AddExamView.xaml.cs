using System.Text.RegularExpressions;
using System.Windows;
using LangLang.Models;
using LangLang.ViewModel;

namespace LangLang.Views
{
    /// <summary>
    /// Interaction logic for ExamView.xaml
    /// </summary>
    public partial class AddExamView : Window
    {
        public AddExamView(Exam? exam = null)
        {
            DataContext = new AddExamViewModel(exam, this);
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
