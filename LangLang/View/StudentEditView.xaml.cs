using System.Windows;
using LangLang.Model;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class StudentEditView : Window
{
    public StudentEditView()
    {
        InitializeComponent();
        DataContext = new StudentEditViewModel(this);
    }
}