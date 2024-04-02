using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    public class CourseListingViewModel : ViewModelBase
    {
        private ObservableCollection<CourseViewModel> _courses;
        private CourseViewModel _selectedItem;
        public ICollectionView CourseCollectionView { get; set; }

        private string _selectedLanguageName;
        private string _selectedLanguageLevel;
        private DateTime _selectedDate;
        private string _selectedDuration;
        private string _selectedFormat;
        private int _teacherID;

        public CourseListingViewModel(int teacherId)
        {
            this.TeacherID = teacherId;
            Language enga1 = new Language("English", LanguageLevel.A1);
            Language enga2 = new Language("English", LanguageLevel.A2);
            Language gera1 = new Language("German", LanguageLevel.A1);
            Language gera2 = new Language("German", LanguageLevel.A2);
            List<Language> peraLangs = new List<Language>
            {
                enga1,
                gera1
            };
            int idT = Teacher.TeacherIds[0];
            _courses = new ObservableCollection<CourseViewModel>
            {
                new CourseViewModel(new Course(new Language("en", LanguageLevel.B1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, idT, new List<int> { 1 })),
                new CourseViewModel(new Course(new Language("ger", LanguageLevel.A1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, idT, new List<int> { 1 })),
                new CourseViewModel(new Course(new Language("in", LanguageLevel.B1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, idT, new List<int> { 1 })),
                new CourseViewModel(new Course(new Language("it", LanguageLevel.C1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, idT, new List<int> { 1 }))
            };
            CoursesCollectionView = CollectionViewSource.GetDefaultView(_courses);

            //foreach (int courseId in Course.CourseIds)
            //{
            //    _courses.Add(new CourseViewModel((Course)Course.GetById(courseId)));
            //}

            CoursesCollectionView.Filter = filterCourses;
            DeleteCommand = new RelayCommand(Delete);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);

        }

        public CourseViewModel SelectedItem
        {
            get =>_selectedItem; 
            set
            {
                _selectedItem = value;
            }
        }

        public ICollectionView CoursesCollectionView { get; }
        public IEnumerable<String> LanguageNameValues => Language.LanguageNames;
        public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        public IEnumerable<String> FormatValues => new List<String>{"online", "in-person"};
        public IEnumerable<CourseViewModel> Courses => _courses;
        public ICommand DeleteCommand { get; }
        public void Delete()
        {
            try
            {
                if (SelectedItem != null)
                {
                    Course course = Course.GetById(SelectedItem.Id);
                    _courses.Remove((CourseViewModel)SelectedItem);

                    Schedule.ModifySchedule(course, course.StartDate, course.Duration, course.Held, null);
                    Course.Courses.Remove(course.Id);
                    Course.CourseIds.Remove(course.Id);
                    MessageBox.Show("Course deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    //obrisi fju
                }
                else
                {
                    MessageBox.Show("Please select an Course to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public ICommand AddCommand { get; }
        public void Add()
        {
            var newWindow = new ModifyCourseView( _courses, CourseCollectionView, TeacherID, null);
            newWindow.Show();
            Application.Current.MainWindow.Closed += (sender, e) =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Application.Current.MainWindow)
                    {
                        window.Close();
                    }
                }
            };
        }

        public ICommand EditCommand { get; }
        public void Edit()
        {
            if (SelectedItem != null)
            {
                var newWindow = new ModifyCourseView(_courses, CourseCollectionView, TeacherID, Course.GetById(SelectedItem.Id));

                newWindow.Show();

            }
            else
            {
                MessageBox.Show("Please select an course to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Application.Current.MainWindow.Closed += (sender, e) =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Application.Current.MainWindow)
                    {
                        window.Close();
                    }
                }
            };
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

        public int TeacherID { get => _teacherID; set => _teacherID = value; }

        private bool filterCourses(object obj)
        {
            if (obj is CourseViewModel courseViewModel)
            {
                return courseViewModel.FilterLanguageName(SelectedLanguageName) &&
                    courseViewModel.FilterLanguageLevel(SelectedLanguageLevel) &&
                    courseViewModel.FilterStartDate(SelectedDate) &&
                    courseViewModel.FilterDuration(SelectedDuration) &&
                    courseViewModel.FilterFormat(SelectedFormat) &&
                    courseViewModel.FilterTeacher(this.TeacherID); 
            }
            else
            {
                return false;
            }
        }

    }
}
