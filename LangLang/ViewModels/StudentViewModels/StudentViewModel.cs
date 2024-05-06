using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.StudentViews;

namespace LangLang.ViewModels.StudentViewModels;

public class StudentViewModel : ViewModelBase
{
    private readonly IUserService _userService = new UserService();
    private readonly IStudentService _studentService = new StudentService();

    private readonly Student _student = UserService.LoggedInUser as Student ?? throw new InvalidInputException("No one is logged in.");

    private readonly Window _studentViewWindow;

    public StudentViewModel(Window studentViewWindow)
    {
        _studentViewWindow = studentViewWindow;

        ViewCoursesCommand = new RelayCommand(ViewCourses);
        ViewAppliedCoursesCommand = new RelayCommand(ViewAppliedCourses);
        ViewExamsCommand = new RelayCommand(ViewExams);
        ViewAppliedExamsCommand = new RelayCommand(ViewAppliedExams);
        DropActiveCourseCommand = new RelayCommand(DropActiveCourse);
        EditAccountCommand = new RelayCommand(EditAccount);
        DeleteAccountCommand = new RelayCommand(DeleteAccount);
        LogOutCommand = new RelayCommand(LogOut);
    }

    public int NumberOfPenaltyPoints => _student.PenaltyPoints;

    public ObservableCollection<Course> AvailableCourses { get; set; }
    public ObservableCollection<Exam> AvailableExams { get; set; }

    public string FullName => $"{_student.FirstName} {_student.LastName}";

    public ICommand ViewCoursesCommand { get; }
    public ICommand ViewAppliedCoursesCommand { get; }
    public ICommand ViewExamsCommand { get; }
    public ICommand ViewAppliedExamsCommand { get; }
    public ICommand DropActiveCourseCommand { get; }
    public ICommand EditAccountCommand { get; }
    public ICommand DeleteAccountCommand { get; }
    public ICommand LogOutCommand { get; }

    private static void ViewCourses()
    {
        new StudentCourseView().Show();
    }

    private static void ViewAppliedCourses()
    {
        new StudentCourseView(true).Show();
    }

    private static void ViewExams()
    {
        new StudentExamView().Show();
    }
    private static void ViewAppliedExams()
    {
        new AppliedExamView().Show();
    }
    
    private void DropActiveCourse()
    {
        try
        {
            var dialog = new DropOutModal();
            if (!dialog.ShowDialog()!.Value) return;
            
            _studentService.DropActiveCourse(_student.Id, dialog.ResponseText);
            MessageBox.Show("Course dropped successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO: Refresh the active course here (after binding it to a view)
        }
        catch (InvalidInputException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void EditAccount()
    {
        // TODO: in teacher, after exam is over remove it from appliedExams, ensure that only future exams are in the list or none at all
        if(_student.AppliedExams.Count != 0)
        {
            MessageBox.Show("You cannot change your data while you have registered exams.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
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