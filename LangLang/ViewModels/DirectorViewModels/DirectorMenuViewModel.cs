using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.CourseViews;
using LangLang.Views.DirectorViews;
using LangLang.Views.TeacherViews;

namespace LangLang.ViewModels.DirectorViewModels
{
    public class DirectorMenuViewModel : ViewModelBase
    {
        private readonly Director _director = UserService.LoggedInUser as Director ?? throw new InvalidInputException("No one is logged in.");
        private readonly Window _directorViewWindow;
        private readonly IUserService _userService = new UserService();

        public DirectorMenuViewModel(Window directorViewWindow)
        {
            _directorViewWindow = directorViewWindow;
            ViewTeachersCommand = new RelayCommand(ViewTeachers);
            ViewCoursesCommand = new RelayCommand(ViewCourses);
            ViewExamsCommand = new RelayCommand(ViewExams);
            SendOutGradesCommand = new RelayCommand(SendOutGrades);
            LogOutCommand = new RelayCommand(LogOut);
        }

        public RelayCommand ViewTeachersCommand { get; }
        public RelayCommand ViewCoursesCommand { get; }
        public RelayCommand ViewExamsCommand { get; }
        public RelayCommand SendOutGradesCommand { get; }
        public ICommand LogOutCommand { get; }

        private void LogOut()
        {
            _userService.Logout();
            new MainWindow().Show();
            _directorViewWindow.Close();
        }

        private void ViewTeachers()
        {
            var teachersView = new TeacherListingView();
            teachersView.Show();
        }
        private void SendOutGrades()
        {
            var sendGradesView = new GradedExams();
            sendGradesView.Show();
        }
        private void ViewExams()
        {

        }
        private void ViewCourses()
        {
            var coursesView = new CoursesListingDirectorView();
            coursesView.Show();
        }
    }
}
