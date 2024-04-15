using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using LangLang.Services;

namespace LangLang.ViewModel
{
    public class CourseListingViewModel : ViewModelBase
    {
        private readonly ITeacherService _teacherService = new TeacherService();
        private readonly ILanguageService _languageService = new LanguageService();
        private readonly ICourseService _courseService = new CourseService();

        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");

        private readonly ObservableCollection<CourseViewModel> _courses;

        private string _selectedLanguageName;
        private string _selectedLanguageLevel;
        private DateTime _selectedDate;
        private string _selectedDuration;
        private string _selectedFormat;

        public CourseListingViewModel()
        {
            _courses = new ObservableCollection<CourseViewModel>(_teacherService.GetCourses(_teacher.Id)
                .Select(course => new CourseViewModel(course)));
            CoursesCollectionView = CollectionViewSource.GetDefaultView(_courses);
            CoursesCollectionView.Filter = FilterCourses;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
        }

        public CourseViewModel? SelectedItem { get; set; }

        public ICollectionView CoursesCollectionView { get; }
        public IEnumerable<String> LanguageNameValues => _languageService.GetAllNames();
        public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        public IEnumerable<String> FormatValues => new List<String> { "online", "in-person" };
        public IEnumerable<CourseViewModel> Courses => _courses;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private void Add()
        {
            var newWindow = new ModifyCourseView();
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

            var newWindow = new ModifyCourseView(_courseService.GetById(SelectedItem.Id));
            newWindow.ShowDialog();
            RefreshCourses();
        }

        private void Delete()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select an Course to delete.", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            Course? course = _courseService.GetById(SelectedItem.Id);
            if (course == null)
            {
                MessageBox.Show("Course doesn't exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _courseService.Delete(course.Id);
            RefreshCourses();

            MessageBox.Show("Course deleted successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        public string SelectedLanguageName
        {
            get => _selectedLanguageName;
            set
            {
                _selectedLanguageName = value;
                CoursesCollectionView.Refresh();
            }
        }

        public string SelectedLanguageLevel
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

        public string SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                _selectedDuration = value;
                CoursesCollectionView.Refresh();
            }
        }

        public string SelectedFormat
        {
            get => _selectedFormat;
            set
            {
                _selectedFormat = value;
                CoursesCollectionView.Refresh();
            }
        }

        private bool FilterCourses(object obj)
        {
            if (obj is CourseViewModel courseViewModel)
            {
                return courseViewModel.FilterLanguageName(SelectedLanguageName) &&
                       courseViewModel.FilterLanguageLevel(SelectedLanguageLevel) &&
                       courseViewModel.FilterStartDate(SelectedDate) &&
                       courseViewModel.FilterDuration(SelectedDuration) &&
                       courseViewModel.FilterFormat(SelectedFormat) &&
                       courseViewModel.FilterTeacher(_teacher.Id);
            }

            return false;
        }

        private void RefreshCourses()
        {
            _courses.Clear();
            _teacherService.GetCourses(_teacher.Id).ForEach(course => _courses.Add(new CourseViewModel(course)));
            CoursesCollectionView.Refresh();
        }
    }
}