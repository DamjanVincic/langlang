using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views;

namespace LangLang.ViewModels
{
    class TeacherMenuViewModel : ViewModelBase
    {
        private readonly IUserService _userService = new UserService();

        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");

        private readonly Window _teacherMenuWindow;

        public TeacherMenuViewModel(Window teacherMenuWindow)
        {
            _teacherMenuWindow = teacherMenuWindow;

            CourseCommand = new RelayCommand(Course);
            ExamCommand = new RelayCommand(Exam);
            LogOutCommand = new RelayCommand(LogOut);
        }

        public ICommand CourseCommand { get; }

        private void Course()
        {
            var newWindow = new CourseView();
            newWindow.Show();
        }

        public ICommand ExamCommand { get; }

        private void Exam()
        {
            var newWindow = new ExamView();
            newWindow.Show();
        }

        public ICommand LogOutCommand { get; }

        private void LogOut()
        {
            _userService.Logout();
            new MainWindow().Show();
            _teacherMenuWindow.Close();
        }
    }
}