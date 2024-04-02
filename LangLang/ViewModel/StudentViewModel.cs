using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;

namespace LangLang.ViewModel;

public class StudentViewModel : ViewModelBase
{
    private Student _student;
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

    private Window _studentViewWindow;
    
    public StudentViewModel(Student student, Window studentViewWindow)
    {
        _studentViewWindow = studentViewWindow;
        _student = student;
        
        ViewCoursesCommand = new RelayCommand(ViewCourses);
        ViewExamsCommand = new RelayCommand(ViewExams);
        EditAccountCommand = new RelayCommand(EditAccount);
        DeleteAccountCommand = new RelayCommand(DeleteAccount);
        LogOutCommand = new RelayCommand(LogOut);
    }

    private void ViewCourses()
    {
        new StudentCourseView().Show();
    }

    private void ViewExams()
    {
        new StudentExamView().Show();
    }

    private void EditAccount()
    {
        new StudentEditView(_student).Show();
    }

    private void DeleteAccount()
    {
        if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            _student.Delete();
            MessageBox.Show("Account deleted successfully");
            new MainWindow().Show();
            _studentViewWindow.Close();
        }
    }

    private void LogOut()
    {
        new MainWindow().Show();
        _studentViewWindow.Close();
    }
}