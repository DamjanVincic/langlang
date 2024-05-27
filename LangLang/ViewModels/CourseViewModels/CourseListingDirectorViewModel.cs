﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.CourseViews;

namespace LangLang.ViewModels.CourseViewModels
{
    public class CourseListingDirectorViewModel : ViewModelBase
    {
        private readonly ITeacherService _teacherService = new TeacherService();
        private readonly ILanguageService _languageService = new LanguageService();
        private readonly ICourseService _courseService = new CourseService();

        private readonly Director _director = UserService.LoggedInUser as Director ??
                                            throw new InvalidOperationException("No one is logged in.");

        private readonly ObservableCollection<CourseViewModel> _courses;

        private string? _selectedLanguageName;
        private string? _selectedLanguageLevel;
        private DateTime _selectedDate;
        private string? _selectedDuration;
        private string? _selectedFormat;

        public CourseListingDirectorViewModel()
        {
            _courses = new ObservableCollection<CourseViewModel>(_courseService.GetAll()
                .Select(course => new CourseViewModel(course)));
            CoursesCollectionView = CollectionViewSource.GetDefaultView(_courses);

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            AddTeacherCommand = new RelayCommand(AddTeacher);
        }

        public CourseViewModel? SelectedItem { get; set; }

        public ICollectionView CoursesCollectionView { get; }
        public IEnumerable<String> LanguageNameValues => _languageService.GetAllNames();
        public static IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        public static IEnumerable<String> FormatValues => new List<String> { "online", "in-person" };
        public IEnumerable<CourseViewModel> Courses => _courses;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddTeacherCommand {  get; }

        private void Add()
        {
            var newWindow = new AddCourseView();
            newWindow.ShowDialog();
            RefreshCourses();
        }

        private void Edit()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select a course to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            new EditCourseView(_courseService.GetById(SelectedItem.Id)).ShowDialog();
            RefreshCourses();
        }

        // TODO: MNOC 3
        private void Delete()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select an Course to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            Course course = _courseService.GetById(SelectedItem.Id) ?? throw new InvalidOperationException("Course doesn't exist.");
            _courseService.Delete(course.Id);
            RefreshCourses();

            MessageBox.Show("Course deleted successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        public void AddTeacher()
        {

            RefreshCourses();
        }


        public string? SelectedLanguageName
        {
            get => _selectedLanguageName;
            set
            {
                _selectedLanguageName = value;
                CoursesCollectionView.Refresh();
            }
        }

        public string? SelectedLanguageLevel
        {
            get => _selectedLanguageLevel;
            set
            {
                _selectedLanguageLevel = value;
                CoursesCollectionView.Refresh();
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                CoursesCollectionView.Refresh();
            }
        }

        public string? SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                _selectedDuration = value;
                CoursesCollectionView.Refresh();
            }
        }

        public string? SelectedFormat
        {
            get => _selectedFormat;
            set
            {
                _selectedFormat = value;
                CoursesCollectionView.Refresh();
            }
        }


        private void RefreshCourses()
        {
            _courses.Clear();
            _courseService.GetAll().ForEach(course => _courses.Add(new CourseViewModel(course)));
            CoursesCollectionView.Refresh();
        }
    }
}