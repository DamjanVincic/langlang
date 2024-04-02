using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    class AddExamViewModel : ViewModelBase
    {
        private Exam _exam;
        private Teacher _loggedInTeacher;
        public AddExamViewModel(Exam exam, Teacher teacher)
        {
            this._exam = exam;
            this._loggedInTeacher = teacher;
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

        public int HourSelected {  get; set; }
        public int MinuteSelected {  get; set; }

        public ICommand EnterExamCommand { get; }

        public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();

        public List<int> Hours => new List<int>() { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24};
        public List<int> Minutes => new List<int>() { 0, 15, 30, 45 };

        private void AddExam()
        {
            try
            {
                // validate
                Language language = IsValidLanguage(Name,LanguageLevel);
                // CanAddScheduleItem(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse)
                if (CanAddScheduleItem(ExamDate, Exam.EXAM_DURATION, new List<Weekday> { (Weekday)ExamDate.DayOfWeek }, _loggedInTeacher.Id, new TimeOnly(HourSelected,MinuteSelected, 0),false,false) && !language.Equals(null))
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
                        Exam exam = new Exam(language, MaxStudents, ExamDate,1, new TimeOnly(HourSelected, MinuteSelected));
                    }
                }
                else
                {
                    MessageBox.Show("Unable to schedule the exam.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
