using System;
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
using LangLang.Views.TeacherViews;
using Teacher = LangLang.Models.Teacher;

namespace LangLang.ViewModels.TeacherViewModels
{
    public class TeacherListingViewModel : ViewModelBase
    {
        private readonly IUserService _userService = new UserService();
        private readonly ITeacherService _teacherService = new TeacherService();
        private readonly ILanguageService _languageService = new LanguageService();
        private readonly ICourseService _courseService = new CourseService();
        private readonly IExamService _examService = new ExamService();

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
            // TODO: Move logic before and after foreach with new window to service
            if (SelectedItem == null)
            {
                MessageBox.Show("No teacher selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Teacher teacher = (Teacher)_userService.GetById(SelectedItem.Id);
            List<Course> ActiveCourses = new List<Course>();
            List<Course> CoursesCreatedByDirector = new List<Course>();
            List<Course> CoursesToBeDeleted = new List<Course>();

            foreach (int courseId in teacher.CourseIds)
            {
                Course course = _courseService.GetById(courseId);
                if (course.AreApplicationsClosed)
                {
                    ActiveCourses.Add(course);
                }
                else
                {
                    if (course.CreatorId != teacher.Id)
                    {
                        CoursesCreatedByDirector.Add(course);
                    }
                    else
                    {
                        CoursesToBeDeleted.Add(course);
                    }
                }
            }

            Dictionary<Course, Teacher> substituteTeachers = new Dictionary<Course, Teacher>();

            //ask for substitute teachers
            foreach (Course course in ActiveCourses)
            {
                List<Teacher> availableTeachers = new List<Teacher>();
                // TODO: uncomment below code
                // foreach(Teacher teacher_ in User.GetTeachers())
                // {
                //     if (Schedule.CanAddScheduleItem(course.StartDate, course.Duration, course.Held, teacher_.Id,
                //             course.ScheduledTime, true, true))
                //     {
                //         availableTeachers.Add(teacher_);
                //     }
                // }
                //TODO: If there are no available teachers, return
                var newWindow = new PickSubstituteTeacherView(availableTeachers, substituteTeachers, course);

                newWindow.ShowDialog();
            }

            foreach (Course course in substituteTeachers.Keys)
            {
                course.TeacherId = substituteTeachers[course].Id;
            }

            foreach (int examId in teacher.ExamIds)
            {
                //shouldn't have id parameter
                _examService.Delete(examId);
            }

            foreach (Course course in CoursesCreatedByDirector)
            {
                course.CreatorId = -1;
            }

            foreach (Course course in CoursesToBeDeleted)
            {
                //delete course TBD
            }

            _teachers.Remove(SelectedItem);
            _userService.Delete(teacher.Id);
            TeachersCollectionView.Refresh();
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