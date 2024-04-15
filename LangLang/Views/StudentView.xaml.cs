using System.Windows;
using LangLang.ViewModel;

namespace LangLang.Views;

public partial class StudentView : Window
{
    public StudentView()
    {
        InitializeComponent();
        DataContext = new StudentViewModel(this);
    }
}