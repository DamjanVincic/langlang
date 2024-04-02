using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    class AddExamViewModel : ViewModelBase
    {
        private Exam _exam;
        public AddExamViewModel(Exam exam)
        {
            this._exam = exam;
            // edit
            if (exam != null)
            {
                Name = this._exam.Language.Name;
                LanguageLevel = this._exam.Language.Level;
                MaxStudents = this._exam.MaxStudents;
                ExamDate = this._exam.ExamDate;
                // ovo treba menjati kad dobijemo listu jezika
                // this._exam =  new Exam(new Language("English", LanguageLevel.A1), 0, DateOnly.FromDateTime(DateTime.Today));

            }

            EnterExamCommand = new RelayCommand(AddExam);
        }
        public string Name { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public int MaxStudents { get; set; }
        public DateOnly ExamDate { get; set; }

        public ICommand EnterExamCommand { get; }

        public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();



        private void AddExam()
        {
            try
            {
                // validate
                Language language = IsValidLanguage(Name,LanguageLevel);
                // CanAddScheduleItem(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse)
                if (CanAddScheduleItem(ExamDate, Exam.EXAM_DURATION, new List<Weekday> { (Weekday)ExamDate.DayOfWeek }, 1, TimeOnly.MaxValue,false,false) && !language.Equals(null))
                {
                    MessageBox.Show("Exam added successfully.", "Success", MessageBoxButton.OK,MessageBoxImage.Information);
                    if (_exam != null)
                    {
                        _exam.Language.Name = Name;
                        _exam.Language.Level = LanguageLevel;
                        _exam.ExamDate = ExamDate;
                        _exam.MaxStudents = MaxStudents;
                    }
                    else
                    {
                        Exam exam = new Exam(language, MaxStudents, ExamDate);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to schedule the exam. The selected date conflicts with an existing exam schedule.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public static bool CanAddScheduleItem(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse, bool isOnline)
        {
            return true;
        }
        public Language IsValidLanguage(string languageName, LanguageLevel level) 
        {
            foreach (Language language in Language.Languages)
            {
                if(language.Name.Equals(languageName) && language.Level.Equals(level))
                {
                    return language;
                }
            }
            return null;
        }
    }
}
