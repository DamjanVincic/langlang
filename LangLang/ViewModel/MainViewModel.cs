using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;

namespace LangLang.ViewModel;

public class MainViewModel : ViewModelBase
{
    private readonly Window _loginWindow;

    public string? Email { get; set; }
    public string? Password { get; set; }

    public ICommand LoginCommand { get; }
    public ICommand NavigateToRegisterCommand { get; }
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToExamViewCommand { get; }
    
    public MainViewModel(Window loginWindow)
    {
        _loginWindow = loginWindow;
        NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
        // NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
        NavigateToExamViewCommand = new RelayCommand(NavigateToExamView);
        LoginCommand = new RelayCommand(Login);
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
            case Teacher teacher:
                new TeacherMenu(teacher).Show();
                _loginWindow.Close();
                Application.Current.MainWindow?.Close();
                break;
        }
    }
}
