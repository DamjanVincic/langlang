﻿using GalaSoft.MvvmLight;
using LangLang.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows.Data;
using System.Windows.Input;
using LangLang.View;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using LangLang.Services;
using Teacher = LangLang.Model.Teacher;

namespace LangLang.ViewModel
{
    public class TeacherListingViewModel : ViewModelBase
    {
        private readonly IUserService _userService = new UserService();
        private readonly ITeacherService _teacherService = new TeacherService();
        private readonly ILanguageService _languageService = new LanguageService();
        private readonly ICourseService _courseService = new CourseService();
        private readonly IExamService _examService = new ExamService();
        private readonly IScheduleService _scheduleService = new ScheduleService();

        private string _selectedLanguageName;
        private string _selectedLanguageLevel;
        private DateTime _selectedDateCreated;

        private readonly ObservableCollection<TeacherViewModel> _teachers;
        private readonly Window _teacherListingWindow;

        public TeacherListingViewModel(Window teacherListingWindow)
        {
            _teacherListingWindow = teacherListingWindow;

            _teachers = new ObservableCollection<TeacherViewModel>(_teacherService.GetAll()
                .Select(teacher => new TeacherViewModel(teacher)));
            TeachersCollectionView = CollectionViewSource.GetDefaultView(_teachers);

            EditCommand = new RelayCommand(OpenEditWindow);
            AddCommand = new RelayCommand(OpenAddWindow);
            DeleteCommand = new RelayCommand(DeleteTeacher);
            LogOutCommand = new RelayCommand(LogOut);

            TeachersCollectionView.Filter = FilterTeachers;
        }

        public ICollectionView TeachersCollectionView { get; }
        public IEnumerable<TeacherViewModel> Teachers => _teachers;
        public IEnumerable<String> LanguageNameValues => _languageService.GetAllNames();
        public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        public ICommand EditCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand LogOutCommand { get; }
        public TeacherViewModel? SelectedItem { get; set; }

        public string SelectedLanguageName
        {
            get => _selectedLanguageName;
            set
            {
                _selectedLanguageName = value;
                TeachersCollectionView.Refresh();
            }
        }

        public string SelectedLanguageLevel
        {
            get => _selectedLanguageLevel;
            set
            {
                _selectedLanguageLevel = value;
                TeachersCollectionView.Refresh();
            }
        }

        public DateTime SelectedDateCreated
        {
            get => _selectedDateCreated;
            set
            {
                _selectedDateCreated = value;
                TeachersCollectionView.Refresh();
            }
        }

        private bool FilterTeachers(object obj)
        {
            if (obj is TeacherViewModel teacherViewModel)
            {
                return teacherViewModel.FilterLanguageName(SelectedLanguageName) &&
                       teacherViewModel.FilterLanguageLevel(SelectedLanguageLevel) &&
                       teacherViewModel.FilterDateCreated(SelectedDateCreated);
            }

            return false;
        }

        private void OpenEditWindow()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No teacher selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_userService.GetById(SelectedItem!.Id) is not Teacher teacher)
            {
                MessageBox.Show("Teacher not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newWindow = new EditTeacherView(teacher);

            newWindow.ShowDialog();
            UpdateTeacherList();
        }

        private void OpenAddWindow()
        {
            var newWindow = new AddTeacherView();
            newWindow.ShowDialog();
            UpdateTeacherList();
        }

        private void DeleteTeacher()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No teacher selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Teacher teacher = (Teacher)_userService.GetById(SelectedItem.Id);
            List<Course> inactiveCoursesCreatedByDirector =
                _teacherService.GetInactiveTeachersCoursesCreatedByDirector(SelectedItem.Id);
            List<Course> inactiveCoursesCreatedByTeacher = _teacherService.GetInactiveCoursesCreatedByTeacher(SelectedItem.Id);

            Dictionary<Course, Teacher> substituteTeachers =
                GetSubstituteTeachers(_teacherService.GetActiveTeachersCourses(SelectedItem.Id));
            //TODO: catch exception when there are no substitute teachers available
            foreach (Course course in substituteTeachers.Keys)
            {
                course.TeacherId = substituteTeachers[course].Id;
            }

            foreach (int examId in teacher.ExamIds)
            {
                //shouldn't have id parameter
                _examService.Delete(examId);
            }

            foreach (Course course in inactiveCoursesCreatedByDirector)
            {
                course.CreatorId = -1;
            }

            foreach (Course course in inactiveCoursesCreatedByDirector)
            {
                //delete course TBD
            }

            _teachers.Remove(SelectedItem);
            _userService.Delete(teacher.Id);
            TeachersCollectionView.Refresh();
        }

        private Dictionary<Course, Teacher> GetSubstituteTeachers(List<Course> activeTeachersCourses)
        {
            Dictionary<Course, Teacher> substituteTeachers = new Dictionary<Course, Teacher>();

            foreach (Course course in activeTeachersCourses)
            {
                List<Teacher> availableTeachers = _teacherService.GetAvailableTeachers(course);
                //TODO: change exception type
                if (!availableTeachers.Any())
                    throw new Exception("There are no available substitute teachers");
                var newWindow = new PickSubstituteTeacherView(availableTeachers, substituteTeachers, course);
                newWindow.ShowDialog();
            }

            return substituteTeachers;
        }

        private void LogOut()
        {
            _userService.Logout();
            new MainWindow().Show();
            _teacherListingWindow.Close();
        }

        private void UpdateTeacherList()
        {
            _teachers.Clear();
            _teacherService.GetAll().ForEach(teacher => _teachers.Add(new TeacherViewModel(teacher)));
            TeachersCollectionView.Refresh();
        }
    }
}