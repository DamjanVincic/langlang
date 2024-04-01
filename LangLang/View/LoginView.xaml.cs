using System.Windows.Controls;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }
}