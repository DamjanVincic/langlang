﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using LangLang.Models;
using LangLang.ViewModels.StudentViewModels;
using LangLang.Services;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace LangLang.ViewModels.TeacherViewModels
{
    public class StartExamViewModel:ViewModelBase
    {
        private readonly int _examId;
        private readonly IExamService _examService = new ExamService();
        private readonly Window _startExamWindow;
        public StartExamViewModel(int examId, Window startExamWindow)
        {
            _examId = examId;
            _startExamWindow = startExamWindow;
            Students = new ObservableCollection<SingleStudentViewModel>(_examService.GetStudents(_examId)
                .Select(student => new SingleStudentViewModel(student)));
            ConfirmCommand = new RelayCommand(Confirm);
        }

        public ObservableCollection<SingleStudentViewModel> Students { get; set; }
        public ICommand ConfirmCommand { get; set; }

        private void Confirm()
        {
            _examService.ConfirmExam(_examId);
            MessageBox.Show("Exam started successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
            _startExamWindow.Close();
        }
    }
}