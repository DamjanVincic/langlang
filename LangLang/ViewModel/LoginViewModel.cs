using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using LangLang.Model;
using LangLang.View;

namespace LangLang.ViewModel;

public class LoginViewModel : ViewModelBase
{
    private readonly Window _loginWindow;
    
    public string? Email { get; set; }
    public string? Password { get; set; }
    
    public ICommand LoginCommand { get; }
    
    public LoginViewModel(Window loginWindow)
    {
        _loginWindow = loginWindow;
        LoginCommand = new RelayCommand(Login);
    }
    
    private void Login()
    {
        User? user = User.Login(Email!, Password!);
        
        switch (user)
        {
            case null:
                MessageBox.Show("Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            case Student student:
                new StudentView(student).Show();
                _loginWindow.Close();
                Application.Current.MainWindow?.Close();
                break;
            case Director:
                new TeachersView().Show();
                _loginWindow.Close();
                Application.Current.MainWindow?.Close();
                break;
        }
    }
}