﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;

namespace LangLang.ViewModels.ExamViewModels
{
    class AddExamViewModel : ViewModelBase
    {
        private readonly ILanguageService _languageService = new LanguageService();
        private readonly IExamService _examService = new ExamService();

        private DateTime _dateSelected;

        private readonly Exam? _exam;

        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");

        private readonly Window _addExamWindow;

        public AddExamViewModel(Exam? exam, Window addExamWindow)
        {
            _exam = exam;
            _addExamWindow = addExamWindow;

            // Edit
            if (_exam is not null)
            {
                Name = _exam.Language.Name;
                LanguageLevel = _exam.Language.Level;
                MaxStudents = _exam.MaxStudents;
                ExamDate = _exam.Date;
                HourSelected = _exam.ScheduledTime.Hour;
                MinuteSelected = _exam.ScheduledTime.Minute;
            }

            EnterExamCommand = new RelayCommand(AddExam);
        }

        public string Name { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public IEnumerable<string> LanguageNames => _languageService.GetAllNames();
        public int MaxStudents { get; set; }
        public DateOnly ExamDate { get; set; }

        public DateTime DateSelected
        {
            get => _dateSelected;
            set
            {
                _dateSelected = value;
                ExamDate = new DateOnly(value.Year, value.Month, value.Day);
                RaisePropertyChanged();
            }
        }

        public int HourSelected { get; set; }
        public int MinuteSelected { get; set; }

        public IEnumerable<LanguageLevel> LanguageLevelValues =>
            Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();

        public List<int> Hours => new()
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

        public List<int> Minutes => new() { 0, 15, 30, 45 };

        public ICommand EnterExamCommand { get; }

        private void AddExam()
        {
            try
            {
                if (_exam is null)
                {
                    _examService.Add(Name, LanguageLevel, MaxStudents, ExamDate, _teacher.Id,
                        new TimeOnly(HourSelected, MinuteSelected));

                    MessageBox.Show("Exam added successfully.", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    _examService.Update(_exam.Id, Name, LanguageLevel, MaxStudents, ExamDate, _teacher.Id,
                        new TimeOnly(HourSelected, MinuteSelected));

                    MessageBox.Show("Exam edited successfully.", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                _addExamWindow.Close();
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}