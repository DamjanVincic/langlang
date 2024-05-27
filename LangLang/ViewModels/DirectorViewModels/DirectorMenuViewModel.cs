using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.DirectorViews;
using LangLang.Views.TeacherViews;

namespace LangLang.ViewModels.DirectorViewModels
{
    public class DirectorMenuViewModel : ViewModelBase
    {
        private readonly Director _director = UserService.LoggedInUser as Director ?? throw new InvalidInputException("No one is logged in.");
        private readonly Window _directorViewWindow;
        private readonly IUserService _userService = new UserService();
        private readonly IDirectorService _directorService = new DirectorService();

        public DirectorMenuViewModel(Window directorViewWindow)
        {
            _directorViewWindow = directorViewWindow;
            ViewTeachersCommand = new RelayCommand(ViewTeachers);
            SendOutGradesCommand = new RelayCommand(SendOutGrades);
            PenaltyPointReportCommand = new RelayCommand(PenaltyPointReport);
            LogOutCommand = new RelayCommand(LogOut);
        }

        public RelayCommand ViewTeachersCommand { get; }
        public RelayCommand SendOutGradesCommand { get; }
        public RelayCommand PenaltyPointReportCommand { get; }
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

        private void PenaltyPointReport()
        {
            _directorService.GeneratePenaltyReport();
            MessageBox.Show("Penalty report generated.");
        }
    }
}
