using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.DirectorViews;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using PdfSharp.Charting;

namespace LangLang.ViewModels.DirectorViewModels
{
    public class DirectorMenuViewModel : ViewModelBase
    {
        private readonly Director _director = UserService.LoggedInUser as Director ?? throw new InvalidInputException("No one is logged in.");
        private readonly Window _directorViewWindow;
        private readonly IUserService _userService = new UserService();
        private readonly IExamService _examService = new ExamService();

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
            // ensure that the directories exist
            string reportsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "data", "reports", "scores and pass rate");
            Directory.CreateDirectory(reportsFolderPath);

            // construct the file path
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
            string filePath = Path.Combine(reportsFolderPath, fileName);

            // create the PDF document
            using (Document document = new Document())
            {
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                Paragraph paragraph = new Paragraph("The average score achieved on each part of all exams in the last year");
                paragraph.Alignment = Element.ALIGN_CENTER; 
                document.Add(paragraph);

                // new line so it is not so close together
                document.Add(Chunk.NEWLINE);


                // set number od cols
                Dictionary<int, List<int>> data = _examService.AveragePointsInLastYear();
                PdfPTable table = new PdfPTable(5);   // exam id and all the points

                // headers
                table.AddCell("Exam");
                table.AddCell("Listening");
                table.AddCell("Talking");
                table.AddCell("Writing");
                table.AddCell("Reading");

                foreach(KeyValuePair<int,List<int>> pair in data)
                {
                    table.AddCell(pair.Key.ToString());
                    foreach(int points in pair.Value)
                    {
                        if (points > 0)
                            table.AddCell(points.ToString());
                        else
                            table.AddCell("not graded");
                    }
                }

                document.Add(table);

                document.Close();

                MessageBox.Show("Report created!.", "Success", MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }
    }
}
