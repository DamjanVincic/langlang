using GalaSoft.MvvmLight;
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
using LangLang.Repositories;
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
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();

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
            try
            {
                if (SelectedItem == null)
                    throw new Exception("No teacher selected");

                PutSubstituteTeachers();
                _userService.Delete(SelectedItem.Id);

                _teachers.Remove(SelectedItem);
                TeachersCollectionView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PutSubstituteTeachers()
        {
            Dictionary<int, Course> activeTeachersCourses = _teacherService.GetCourses(SelectedItem.Id)
                .Where(course => course.AreApplicationsClosed)
                .ToDictionary(course => course.Id);

            Dictionary<int, int> substituteTeacherIds = new Dictionary<int, int>();

            foreach (int courseId in activeTeachersCourses.Keys)
            {
                substituteTeacherIds[courseId]= PickSubstituteTeacher(activeTeachersCourses[courseId]);
            }

            foreach (int courseId in substituteTeacherIds.Keys)
            {
                activeTeachersCourses[courseId].TeacherId=substituteTeacherIds[courseId];
                _courseRepository.Update(activeTeachersCourses[courseId]);
            }
        }

        private int PickSubstituteTeacher(Course course)
        {
            List<Teacher> availableTeachers = _teacherService.GetAvailableTeachers(course);

            if (!availableTeachers.Any())
                throw new Exception("There are no available substitute teachers");

            int substituteTeacherId = -1;
            var newWindow = new PickSubstituteTeacherView(availableTeachers, ref substituteTeacherId, course);
            newWindow.ShowDialog();
            return substituteTeacherId;
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