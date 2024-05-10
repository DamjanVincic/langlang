﻿using GalaSoft.MvvmLight;
using LangLang.Services;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using LangLang.ViewModels.StudentViewModels;

namespace LangLang.ViewModels.CourseViewModels
{
    class StartCourseViewModel : ViewModelBase
    {
        private readonly int _courseId;
        private readonly ICourseService _courseService = new CourseService();
        private readonly IStudentService _studentService = new StudentService();
        private readonly Window _startCourseWindow;
        public StartCourseViewModel(int courseId, Window startCourseWindow)
        {
            _courseId = courseId;
            _startCourseWindow = startCourseWindow;
            Students = new ObservableCollection<SingleStudentViewModel>(_courseService.GetStudents(_courseId)
                .Select(student => new SingleStudentViewModel(student)));
            ConfirmCommand = new RelayCommand(Confirm);
            RejectApplicationCommand = new RelayCommand(RejectApplication);

        }

        public ObservableCollection<SingleStudentViewModel> Students { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand? RejectApplicationCommand { get; }
        public SingleStudentViewModel? SelectedItem { get; set; }
        public string? RejectionReason { get; set; }

        private void Confirm()
        {
            _courseService.ConfirmCourse(_courseId);
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
            _courseService.RejectStudentsApplication(_courseId, SelectedItem.Id);
            //TODO
            //_studentService.SendNotiffication();
            
            MessageBox.Show("Student rejected successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
