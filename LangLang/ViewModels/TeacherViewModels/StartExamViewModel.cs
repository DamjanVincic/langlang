using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using LangLang.Models;
using LangLang.ViewModels.StudentViewModels;
using LangLang.Services;

namespace LangLang.ViewModels.TeacherViewModels
{
    public class StartExamViewModel:ViewModelBase
    {
        private readonly IExamService _examService = new ExamService();
        public StartExamViewModel(int examId)
        {
            Students = new ObservableCollection<SingleStudentViewModel>(_examService.GetStudents(examId)
                .Select(student => new SingleStudentViewModel(student)));
        }

        public ObservableCollection<SingleStudentViewModel> Students { get; set; }
    }
}
