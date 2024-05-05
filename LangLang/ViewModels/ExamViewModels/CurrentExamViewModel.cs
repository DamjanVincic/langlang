using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Repositories;
using LangLang.Services;
using LangLang.ViewModels.StudentViewModels;
using LangLang.Views.ExamViews;

namespace LangLang.ViewModels.ExamViewModels
{
    public class CurrentExamViewModel:ViewModelBase
    {
        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");

        private readonly IUserRepository _userRepository = new UserFileRepository();
        private readonly IExamGradeRepository _examGradeRepository = new ExamGradeFileRepository();
        private readonly IExamService _examService = new ExamService();
        private readonly Exam _exam;
        private readonly Window _currentWindow;

        public CurrentExamViewModel(Window currentWindow)
        {
            _exam = _examService.GetCurrentExam(_teacher.Id);
            RefreshStudents();
            AddExamGradeCommand = new RelayCommand(AddExamGrade);
            FinishExamCommand = new RelayCommand(FinishExam);
            _currentWindow=currentWindow;
        }

        public ObservableCollection<StudentExamGradeViewModel> Students { get; set; } = new();
        public StudentExamGradeViewModel SelectedItem { get; set; }

        public ICommand AddExamGradeCommand { get; set; }
        public ICommand FinishExamCommand { get; set; }

        private void AddExamGrade()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No student selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newWindow = new AddExamGradeView(SelectedItem.StudentId, _exam.Id);
            newWindow.ShowDialog();
            RefreshStudents();
        }

        private void FinishExam()
        {
            try
            {
                _examService.CheckGrades(_exam.Id);
                MessageBox.Show("Successfully finished exam.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _currentWindow.Close();
            }
            catch (InvalidInputException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshStudents()
        {
            Students.Clear();
            foreach (int studentId in _exam.StudentIds)
            {
                Student student = _userRepository.GetById(studentId) as Student ??
                                  throw new InvalidInputException("Student doesn't exist.");
                ExamGrade examGrade;
                if (student.ExamGradeIds.ContainsKey(_exam.Id))
                    examGrade = _examGradeRepository.GetById(student.ExamGradeIds[_exam.Id]) ??
                                throw new InvalidInputException("Exam grade doesn't exist");
                else
                    examGrade = null;

                Students.Add(new StudentExamGradeViewModel(student,examGrade));
            }
        }
    }
}
