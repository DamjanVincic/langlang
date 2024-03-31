using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;

namespace LangLang.ViewModel;

public class LoginViewModel : ViewModelBase
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    public ICommand LoginCommand { get; }
    
    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(Login);
    }
    
    private void Login()
    {
        User? user = User.Login(Email, Password);
        
        switch (user)
        {
            case null:
                MessageBox.Show("Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                break;
            case Student:
                throw new NotImplementedException(); // else if (user is Teacher) { } ...
        }
    }
}