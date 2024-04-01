using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LangLang.Commands;
using LangLang.Model;

namespace LangLang.ViewModel
{
    public class ExamViewModel : ViewModelBase
    {
        // glues the model and the view
        private readonly Exam exam;

        public string Id => exam.Id.ToString();
        public string Language { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public string MaxStudents => exam.MaxStudents.ToString();
        public DateOnly ExamDate => exam.ExamDate;


        public event PropertyChangedEventHandler? PropertyChanged;

        public ExamViewModel(Exam exam)
        {
            this.exam = exam;
        }

    }
}
