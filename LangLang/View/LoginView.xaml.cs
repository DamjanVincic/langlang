using System.Windows;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = new LoginViewModel(this);
    }
}