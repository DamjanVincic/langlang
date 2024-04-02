using System.Windows;
using LangLang.Model;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class StudentView : Window
{
    public StudentView(Student student)
    {
        InitializeComponent();
        DataContext = new StudentViewModel(student, this);
    }
}