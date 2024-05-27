using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Services.ReportServices;
using LangLang.Views.DirectorViews;
using LangLang.Views.TeacherViews;

namespace LangLang.ViewModels.DirectorViewModels
{
    public class DirectorMenuViewModel : ViewModelBase
    {
        private readonly Director _director = UserService.LoggedInUser as Director ?? throw new InvalidInputException("No one is logged in.");
        private readonly Window _directorViewWindow;
        private readonly IUserService _userService = new UserService();
        private readonly IGradeReportService _gradeReportService = new GradeReportService();

        public DirectorMenuViewModel(Window directorViewWindow)
        {
            _directorViewWindow = directorViewWindow;
            ViewTeachersCommand = new RelayCommand(ViewTeachers);
            SendOutGradesCommand = new RelayCommand(SendOutGrades);
            LogOutCommand = new RelayCommand(LogOut);
            PenaltyPointReportCommand = new RelayCommand(GeneratePenaltyPointReport);
            GradeReportCommand = new RelayCommand(GenerateGradeReport);
            PointReportCommand = new RelayCommand(GeneratePointReport);
            LanguageReportCommand = new RelayCommand(GenerateLanguageReport);
        }

        public RelayCommand ViewTeachersCommand { get; }
        public RelayCommand SendOutGradesCommand { get; }
        public ICommand PenaltyPointReportCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand GradeReportCommand { get; }
        public ICommand PointReportCommand { get; }
        public ICommand LanguageReportCommand { get; }

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
        private void GeneratePenaltyPointReport()
        {
            throw new NotImplementedException();
        }
        private void GenerateGradeReport()
        {
            _gradeReportService.GenerateGradeReport();
        }
        private void GeneratePointReport()
        {
            throw new NotImplementedException();
        }
        private void GenerateLanguageReport()
        {
            throw new NotImplementedException();
        }
    }
}
