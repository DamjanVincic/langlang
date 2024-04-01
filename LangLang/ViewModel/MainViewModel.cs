using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.View;

namespace LangLang.ViewModel;

public class MainViewModel : ViewModelBase
{
    public ICommand NavigateToRegisterCommand { get; }
    
    private readonly NavigationService _navigationService;
    
    public MainViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
    }

    public void NavigateToRegister()
    {
        _navigationService.Navigate(new RegisterView());
    }
}