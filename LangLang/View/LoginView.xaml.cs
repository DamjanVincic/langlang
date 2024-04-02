using System.Windows;
using System.Windows.Controls;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }
}