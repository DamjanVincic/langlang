using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LangLang.Model;

namespace LangLang.ViewModel
{
    public class ExamViewModel : ViewModelBase
    {
        // glues the model and the view
        private Exam exam;

        public string Id => exam.Id.ToString();
        public string Language { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public string MaxStudents => exam.MaxStudents.ToString();
        public DateOnly ExamDate => exam.ExamDate;

        public ExamListingViewModel ObjExamListingViewModel;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ExamViewModel(Exam exam)
        {
            this.exam = exam;
        }

        public bool FilterLevel(string level)
        {
            if (level == null)
            {
                return true;
            }
            return exam.Language.Level == (LanguageLevel)Enum.Parse(typeof(LanguageLevel), level);
        }

        public bool FilterLanguageName(string name)
        {
            if (name == null)
            {
                return true;
            }
            return exam.Language.Name == name;
        }
        public bool FilterDateHeld(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return true;
            }
            DateOnly chosenDate = new DateOnly(date.Year, date.Month, date.Day);
            return chosenDate == exam.ExamDate;
        }

    }
}
