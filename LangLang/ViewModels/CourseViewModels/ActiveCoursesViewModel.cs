using LangLang.Models;
using LangLang.Services;
using LangLang.Views.CourseViews;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace LangLang.ViewModels.CourseViewModels
{
    class ActiveCoursesViewModel : ViewModelBase
    {
        private readonly ICourseService _courseService = new CourseService();
        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");
        public ActiveCoursesViewModel()
        {
            ActiveCourses = new ObservableCollection<CourseViewModel>();
            RefreshActiveCourses();
            SeeStudentsListCommand = new RelayCommand(SeeStudentsList);
        }

        public ObservableCollection<CourseViewModel> ActiveCourses { get; set; }
        public CourseViewModel? SelectedItem { get; set; }
        public ICommand SeeStudentsListCommand { get; set; }

        private void SeeStudentsList()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No course selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newWindow = new CurrentCourseView(SelectedItem.Id);

            newWindow.ShowDialog();
            RefreshActiveCourses();
        }

        private void RefreshActiveCourses()
        {
            ActiveCourses.Clear();
            _courseService.GetActiveCourses(_teacher.Id).ForEach(course => ActiveCourses.Add(new CourseViewModel(course)));
        }
    }
}

