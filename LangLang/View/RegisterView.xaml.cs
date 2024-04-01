using System.Windows;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class RegisterView : Window
{
    public RegisterView()
    {
        InitializeComponent();
        DataContext = new RegisterViewModel();
    }
}