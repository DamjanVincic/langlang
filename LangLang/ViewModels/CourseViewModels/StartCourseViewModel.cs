﻿using GalaSoft.MvvmLight;
using LangLang.Services;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using LangLang.Models;
using LangLang.ViewModels.StudentViewModels;
using LangLang.Models;

namespace LangLang.ViewModels.CourseViewModels
{
    class StartCourseViewModel : ViewModelBase
    {
        private readonly ITeacherService _teacherService = new TeacherService();
        
        private readonly int _courseId;
        private readonly ICourseService _courseService = new CourseService();
        private readonly IStudentService _studentService = new StudentService();
        private readonly Window _startCourseWindow;
        public StartCourseViewModel(int courseId, Window startCourseWindow)
        {
            _courseId = courseId;
            _startCourseWindow = startCourseWindow;
            Students = new ObservableCollection<StudentViewModel>(_courseService.GetStudents(_courseId)
                .Select(student => new StudentViewModel(student)));
            ConfirmCommand = new RelayCommand(Confirm);
            RejectApplicationCommand = new RelayCommand(RejectApplication);

        }

        public ObservableCollection<StudentViewModel> Students { get; set; }
        public StudentViewModel? SelectedStudent { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand? RejectApplicationCommand { get; }
        public StudentViewModel? SelectedItem { get; set; }
        public string? RejectionReason { get; set; }

        private void Confirm()
        {
            _teacherService.ConfirmCourse(_courseId);
            MessageBox.Show("Course started successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
            _startCourseWindow.Close();

        }
        
        private void RejectApplication()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No student selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(RejectionReason))
            {
                MessageBox.Show("Must input the reason for rejection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            try
            {
                _teacherService.RejectStudentApplication(_courseId, SelectedStudent!.Id);
                MessageBox.Show("Student application rejected.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Students.Remove(SelectedItem);
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

