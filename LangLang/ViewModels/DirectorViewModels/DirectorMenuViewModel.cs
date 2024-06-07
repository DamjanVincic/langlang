﻿using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Repositories;
using LangLang.Services;
using LangLang.Views.CourseViews;
using LangLang.Services.ReportServices;
using LangLang.Views.DirectorViews;
using LangLang.Views.ExamViews;

namespace LangLang.ViewModels.DirectorViewModels
{
    public class DirectorMenuViewModel : ViewModelBase
    {
        private readonly Director _director = UserService.LoggedInUser as Director ?? throw new InvalidInputException("No one is logged in.");
        private readonly Window _directorViewWindow;

        private readonly IGradeReportService _gradeReportService =  ServiceProvider.GetRequiredService<IGradeReportService>();
        private readonly IPassRateReportService _passRateReportService = ServiceProvider.GetRequiredService<IPassRateReportService>();
        private readonly ILanguageReportService _languageReportService = ServiceProvider.GetRequiredService<ILanguageReportService>();
        private readonly IUserService _userService = ServiceProvider.GetRequiredService<IUserService>();
        private readonly IDirectorService _directorService = ServiceProvider.GetRequiredService<IDirectorService>();
        
        public DirectorMenuViewModel(Window directorViewWindow)
        {
            _directorViewWindow = directorViewWindow;
            
            ViewTeachersCommand = new RelayCommand(ViewTeachers);
            ViewCoursesCommand = new RelayCommand(ViewCourses);
            ViewExamsCommand = new RelayCommand(ViewExams);
            SendOutGradesCommand = new RelayCommand(SendOutGrades);
            NotifyBestStudentsCommand = new RelayCommand(NotifyBestStudents);
            LogOutCommand = new RelayCommand(LogOut);
            PenaltyPointReportCommand = new RelayCommand(GeneratePenaltyPointReport);
            GradeReportCommand = new RelayCommand(GenerateGradeReport);
            PointReportCommand = new RelayCommand(GeneratePointReport);
            LanguageReportCommand = new RelayCommand(GenerateLanguageReport);
        }

        public RelayCommand ViewTeachersCommand { get; }
        public RelayCommand ViewCoursesCommand { get; }
        public RelayCommand ViewExamsCommand { get; }
        public RelayCommand SendOutGradesCommand { get; }
        public RelayCommand PenaltyPointReportCommand { get; }
        public RelayCommand NotifyBestStudentsCommand { get; }
        public RelayCommand LogOutCommand { get; }
        public RelayCommand GradeReportCommand { get; }
        public RelayCommand PointReportCommand { get; }
        public RelayCommand LanguageReportCommand { get; }

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
            var examView = new AddExamView();
            examView.Show();
        }
        private void ViewCourses()
        {
            var coursesView = new CoursesListingDirectorView();
            coursesView.Show();
        }

        private void GeneratePenaltyPointReport()
        {
            _directorService.GeneratePenaltyReport();
            MessageBox.Show("Penalty report generated.");
        }
        private void GenerateGradeReport()
        {
            try
            {
                _gradeReportService.GenerateGradeReport();
                MessageBox.Show("Report sent successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GeneratePointReport()
        {
            _passRateReportService.GeneratePointsPassRateReport();
        }
        private void GenerateLanguageReport()
        {
            try
            {
                _languageReportService.GenerateLanguageReport();
                MessageBox.Show("Report sent successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void NotifyBestStudents()
        {
            new BestStudentsNotificationView().ShowDialog();

        }
    }
}