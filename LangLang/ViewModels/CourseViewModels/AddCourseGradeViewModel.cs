﻿using GalaSoft.MvvmLight;
using LangLang.Models;
using LangLang.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace LangLang.ViewModels.CourseViewModels
{
    internal class AddCourseGradeViewModel : ViewModelBase
    {
        private readonly int _studentId;
        private readonly int _courseId;
        private readonly Window _currentWindow;

        private readonly IStudentService _studentService = new StudentService();

        public AddCourseGradeViewModel(int studentId, int courseId, Window currentWindow)
        {
            _studentId = studentId;
            _courseId = courseId;
            AddCourseGradeCommand = new RelayCommand(AddCourseGrade);
            _currentWindow = currentWindow;
        }

        public int KnowledgeGrade { get; set; }
        public int ActivityGrade { get; set; }

        public ICommand AddCourseGradeCommand { get; set; }

        private void AddCourseGrade()
        {
            try
            {
                _studentService.AddCourseGrade(_studentId, _courseId, KnowledgeGrade, ActivityGrade);
                MessageBox.Show("Grade added successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _currentWindow.Close();
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
