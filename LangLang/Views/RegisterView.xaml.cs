using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views;

public partial class RegisterView : Window
{
    public RegisterView()
    {
        InitializeComponent();
        DataContext = new RegisterViewModel(this);
    }
}