﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LangLang.Models;
using LangLang.Repositories;
using LangLang.Services;

namespace LangLang.ViewModels.TeacherViewModels
{
    internal class PickSubstituteTeacherViewModel : ViewModelBase
    {
        private readonly IUserService _userService = new UserService();
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();
        private readonly ObservableCollection<TeacherViewModel> _displayedTeachers;
        private readonly Course _course;

        public PickSubstituteTeacherViewModel(List<Teacher> availableTeachers, Course course)
        {
            Title = $"Select substitute teacher for course {course.Language}";
            _course = course;

            _displayedTeachers = new ObservableCollection<TeacherViewModel>(
                availableTeachers.Select(teacher => new TeacherViewModel(teacher)));

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
            _course.TeacherId = _userService.GetById(SelectedItem.Id).Id;
            _courseRepository.Update(_course);
            MessageBox.Show("Substitute teacher picked successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}