using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views;

public partial class StudentEditView : Window
{
    public StudentEditView()
    {
        InitializeComponent();
        DataContext = new StudentEditViewModel(this);
    }
}