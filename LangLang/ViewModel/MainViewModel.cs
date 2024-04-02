using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;

namespace LangLang.ViewModel;

public class MainViewModel : ViewModelBase
{
    public ICommand NavigateToRegisterCommand { get; }
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToExamViewCommand { get; }
    
    public MainViewModel()
    {
        NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
        NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
        NavigateToExamViewCommand = new RelayCommand(NavigateToExamView);
    }

    public void NavigateToRegister()
    {
        new RegisterView().Show();
    }

    public void NavigateToLogin()
    {
        new LoginView().Show();
    }
    
    private void NavigateToExamView()
    {
        new ExamView().Show();
    }
}
