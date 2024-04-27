using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.Services;
using LangLang.View;

namespace LangLang.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly IUserService _userService = new UserService();
    
    private readonly Window _loginWindow;

    public string? Email { get; set; }
    public string? Password { get; set; }

    public ICommand LoginCommand { get; }
    public ICommand NavigateToRegisterCommand { get; }
    
    public MainViewModel(Window loginWindow)
    {
        _loginWindow = loginWindow;
        NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
        LoginCommand = new RelayCommand(Login);
    }

    private void NavigateToRegister()
    {
        new RegisterView().Show();
    }
    
    private void Login()
    {
        User? user = _userService.Login(Email!, Password!);

        switch (user)
        {
            case null:
                MessageBox.Show("Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            case Student:
                _userService.CheckIfFirstInMonth();
                new StudentView().Show();
                break;
            case Director:
                new TeachersView().Show();
                break;
            case Teacher:
                new TeacherMenu().Show();
                break;
        }

        _loginWindow.Close();
        Application.Current.MainWindow?.Close();
    }
}
