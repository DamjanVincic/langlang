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
                _exam.Language.Name = Name;
                _exam.Language.Level = LanguageLevel;
                _exam.ExamDate = ExamDate;
                _exam.MaxStudents = MaxStudents;
                Exam exam = new Exam(new Language(Name, LanguageLevel), MaxStudents, ExamDate);
                if (CheckExamSchedule(exam))
                {
                    MessageBox.Show("Exam added successfully.", "Success", MessageBoxButton.OK,MessageBoxImage.Information);
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

        private bool CheckExamSchedule(Exam exam)
        {
            return true;
        }
    }
}
