using System.Windows;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class StudentView : Window
{
    public StudentView()
    {
        InitializeComponent();
        DataContext = new StudentViewModel(this);
    }
}