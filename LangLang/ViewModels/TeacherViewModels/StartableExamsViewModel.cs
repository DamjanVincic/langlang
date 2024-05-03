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
using LangLang.Services;
using LangLang.ViewModels.ExamViewModels;
using LangLang.ViewModels.StudentViewModels;
using LangLang.Views.TeacherViews;

namespace LangLang.ViewModels.TeacherViewModels
{
    internal class StartableExamsViewModel:ViewModelBase
    {
        private readonly IExamService _examService = new ExamService();

        public StartableExamsViewModel()
        {
            //TODO: get startable exams only for current teacher
            StartableExams = new ObservableCollection<ExamViewModel>(_examService.GetStartableExams()
                .Select(exam => new ExamViewModel(exam)));
            StartExamCommand = new RelayCommand(StartExam);

        }

        public ObservableCollection<ExamViewModel> StartableExams { get; set; }
        public ExamViewModel? SelectedItem { get; set; }
        public ICommand StartExamCommand { get; set; }

        private void StartExam()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No exam selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newWindow = new StartExamView(SelectedItem.Id);

            newWindow.ShowDialog();
            //TODO: update exam list 
        }
    }
}
