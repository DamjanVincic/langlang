using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public CurrentExamViewModel()
        {
            //TODO: try catch
            _exam = _examService.GetCurrentExam(_teacher.Id);
            RefreshStudents();
            AddExamGradeCommand = new RelayCommand(AddExamGrade);
        }

        public ObservableCollection<StudentExamGradeViewModel> Students { get; set; } = new();
        public StudentExamGradeViewModel SelectedItem { get; set; }

        public ICommand AddExamGradeCommand { get; set; }

        private void AddExamGrade()
        {
            var newWindow = new AddExamGradeView(SelectedItem.StudentId, _exam.Id);
            newWindow.ShowDialog();
            RefreshStudents();
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
