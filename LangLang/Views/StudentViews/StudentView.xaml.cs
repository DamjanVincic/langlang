using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views.StudentViews;

public partial class StudentView : Window
{
    public StudentView()
    {
        InitializeComponent();
        DataContext = new StudentViewModel(this);
    }
}