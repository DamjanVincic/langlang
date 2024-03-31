using System.Windows.Controls;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();
        DataContext = new RegisterViewModel();
    }
}