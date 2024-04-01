using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LangLang.Model;

namespace LangLang.ViewModel
{
    public class ExamViewModel : ViewModelBase
    {
        // glues the model and the view
        private Exam _exam;
        public ExamViewModel(Exam exam)
        {
            this._exam = exam;
        }

        public int Id => _exam.Id;
        public string Language => _exam.Language.Name;
        public LanguageLevel LanguageLevel => _exam.Language.Level;
        public string MaxStudents => _exam.MaxStudents.ToString();
        public DateOnly ExamDate => _exam.ExamDate;

        public bool FilterLevel(string level)
        {
            if (level == null)
            {
                return true;
            }
            return _exam.Language.Level == (LanguageLevel)Enum.Parse(typeof(LanguageLevel), level);
        }

        public bool FilterLanguageName(string name)
        {
            if (name == null)
            {
                return true;
            }
            return _exam.Language.Name == name;
        }
        public bool FilterDateHeld(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return true;
            }
            DateOnly chosenDate = new DateOnly(date.Year, date.Month, date.Day);
            return chosenDate == _exam.ExamDate;
        }

    }
}
