using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.Services;
using LangLang.View;

namespace LangLang.ViewModel;

public class StudentViewModel : ViewModelBase
{
    private readonly IUserService _userService = new UserService();
    
    private readonly Student _student = UserService.LoggedInUser as Student ?? throw new InvalidInputException("No one is logged in.");

    private readonly Window _studentViewWindow;
    
    public StudentViewModel(Window studentViewWindow)
    {
        _studentViewWindow = studentViewWindow;
        
        ViewCoursesCommand = new RelayCommand(ViewCourses);
        ViewExamsCommand = new RelayCommand(ViewExams);
        EditAccountCommand = new RelayCommand(EditAccount);
        DeleteAccountCommand = new RelayCommand(DeleteAccount);
        LogOutCommand = new RelayCommand(LogOut);
    }
    
    public ObservableCollection<Course> AvailableCourses { get; set; }
    public ObservableCollection<Exam> AvailableExams { get; set; }
    
    public string FullName => $"{_student.FirstName} {_student.LastName}";

    public ICommand ViewCoursesCommand { get; }
    public ICommand ViewExamsCommand { get; }
    public ICommand EditAccountCommand { get; }
    public ICommand DeleteAccountCommand { get; }
    public ICommand LogOutCommand { get; }
    public ICommand ApplyForCourseCommand { get; }
    public ICommand ApplyForExamCommand { get; }

    private static void ViewCourses()
    {
        new StudentCourseView().Show();
    }

    private static void ViewExams()
    {
        new StudentExamView().Show();
    }

    private void EditAccount()
    {
        new StudentEditView().ShowDialog();
    }

    private void DeleteAccount()
    {
        if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
        
        _userService.Delete(_student.Id);
        MessageBox.Show("Account deleted successfully");
        new MainWindow().Show();
        _studentViewWindow.Close();
    }

    private void LogOut()
    {
        _userService.Logout();
        new MainWindow().Show();
        _studentViewWindow.Close();
    }
}