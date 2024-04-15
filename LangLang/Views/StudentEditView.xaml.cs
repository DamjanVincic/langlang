using System.Windows;
using LangLang.ViewModel;

namespace LangLang.Views;

public partial class StudentEditView : Window
{
    public StudentEditView()
    {
        InitializeComponent();
        DataContext = new StudentEditViewModel(this);
    }
}