using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    class AddExamViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public int MaxStudents { get; set; }
        public DateOnly ExamDate { get; set; }

        public ICommand EnterExamCommand { get; }

        public AddExamViewModel()
        {
            EnterExamCommand = new RelayCommand(AddExam);
        }

        private void AddExam()
        {
            try
            {
                Exam exam = new Exam(new Language(Name, LanguageLevel), MaxStudents, ExamDate);
                if (checkExamSchedule(exam))
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

        private bool checkExamSchedule(Exam exam)
        {
            return true;
        }
    }
}
