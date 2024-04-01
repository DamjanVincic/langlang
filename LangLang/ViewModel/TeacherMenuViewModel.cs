using GalaSoft.MvvmLight;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    class TeacherMenuViewModel : ViewModelBase
    {
        private Exam _exam;
        public string Name { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public int MaxStudents { get; set; }
        public DateOnly ExamDate { get; set; }

        public ICommand EnterExamCommand { get; }

        public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();

        public TeacherMenuViewModel(Exam exam)
        {
            // If exam is null, initialize it with default values
            if (exam == null)
            {
                // ovo treba menjati kad dobijemo listu jezika
                this._exam = new Exam(new Language("English", LanguageLevel.A1), 0, DateOnly.FromDateTime(DateTime.Today));

            }
            else
            {
                this._exam = exam;
            }

            EnterExamCommand = new RelayCommand(AddExam);
            // Assigning properties from the exam object
            Name = this._exam.Language.Name;
            LanguageLevel = this._exam.Language.Level;
            MaxStudents = this._exam.MaxStudents;
            ExamDate = this._exam.ExamDate;
        }

    }
