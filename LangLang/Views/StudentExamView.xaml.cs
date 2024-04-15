using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views;

public partial class StudentExamView : Window
{
    public StudentExamView()
    {
        InitializeComponent();
        DataContext = new StudentExamViewModel();
    }
}