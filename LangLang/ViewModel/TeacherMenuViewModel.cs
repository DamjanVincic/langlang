﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    class TeacherMenuViewModel : ViewModelBase
    {
        private Window _teacherMenuWindow;
        private Teacher _teacher;
        
        public TeacherMenuViewModel(Teacher teacher, Window teacherMenuWindow)
        {
            _teacher = teacher;
            _teacherMenuWindow = teacherMenuWindow;
            CourseCommand = new RelayCommand(Course);
            ExamCommand = new RelayCommand(Exam);
            LogOutCommand = new RelayCommand(LogOut);
        }

        public ICommand CourseCommand { get; }
        public void Course()
        {
            var newWindow = new CourseView(_teacher);
            newWindow.Show();
        }
        public ICommand ExamCommand { get; }
        public void Exam()
        {
            var newWindow = new ExamView(_teacher);
            newWindow.Show();
        }
        
        public ICommand LogOutCommand { get; }

        private void LogOut()
        {
            new MainWindow().Show();
            _teacherMenuWindow.Close();
        }
    }
}