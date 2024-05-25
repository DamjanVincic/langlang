using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.DirectorViews;
using LangLang.Views.TeacherViews;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

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
            SendOutGradesCommand = new RelayCommand(SendOutGrades);
            LogOutCommand = new RelayCommand(LogOut);
            ScoreReportCommand = new RelayCommand(ScoreReport);
        }

        public RelayCommand ViewTeachersCommand { get; }
        public RelayCommand SendOutGradesCommand { get; }
        public ICommand LogOutCommand { get; }
        public RelayCommand ScoreReportCommand { get; }

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
        private void ScoreReport()
        {
            //ensure that the directories exist
            string reportsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "reports", "scores and pass rate");
            Directory.CreateDirectory(reportsFolderPath);

            //construct the file path
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
            string filePath = Path.Combine(reportsFolderPath, fileName);

            //create the PDF document
            using (iTextSharp.text.Document document = new iTextSharp.text.Document())
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                Paragraph paragraph = new Paragraph("Hello, world!");
                document.Add(paragraph);

                document.Close();

                MessageBox.Show("Report created!.", "Success", MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }
    }
}
