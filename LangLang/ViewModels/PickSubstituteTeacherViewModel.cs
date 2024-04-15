using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LangLang.Models;
using LangLang.Services;

namespace LangLang.ViewModels
{
    internal class PickSubstituteTeacherViewModel : ViewModelBase
    {
        private readonly IUserService _userService = new UserService();

        private readonly ObservableCollection<TeacherViewModel> _displayedTeachers = new();
        private readonly Dictionary<Course, Teacher> _substituteTeachers;
        private readonly Course _course;

        public PickSubstituteTeacherViewModel(List<Teacher> availableTeachers,
            Dictionary<Course, Teacher> substituteTeachers, Course course)
        {
            Title = $"Select substitute teacher for course {course.Language}";

            _substituteTeachers = substituteTeachers;
            _course = course;

            foreach (Teacher teacher in availableTeachers)
                _displayedTeachers.Add(new TeacherViewModel(teacher));

            TeachersCollectionView = CollectionViewSource.GetDefaultView(_displayedTeachers);
            SaveCommand = new RelayCommand(SaveSubstitute);
        }

        public ICollectionView TeachersCollectionView { get; }
        public ICommand SaveCommand { get; }
        public string Title { get; set; }
        public TeacherViewModel? SelectedItem { get; set; }

        private void SaveSubstitute()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No teacher selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Teacher? teacher = _userService.GetById(SelectedItem.Id) as Teacher;
            if (teacher == null)
            {
                MessageBox.Show("User doesn't exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _substituteTeachers[_course] = teacher!;
            MessageBox.Show("Substitute teacher picked successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}