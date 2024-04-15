using System.Windows;
using LangLang.ViewModel;

namespace LangLang.Views;

public partial class StudentExamView : Window
{
    public StudentExamView()
    {
        InitializeComponent();
        DataContext = new StudentExamViewModel();
    }
}