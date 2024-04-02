using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.View;

namespace LangLang.ViewModel;

public class MainViewModel : ViewModelBase
{
    public ICommand NavigateToRegisterCommand { get; }
    public ICommand NavigateToLoginCommand { get; }
    
    public MainViewModel()
    {
        NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
        NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
    }

    public void NavigateToRegister()
    {
        new RegisterView().Show();
    }

    public void NavigateToLogin()
    {
        new LoginView().Show();
    }
}
