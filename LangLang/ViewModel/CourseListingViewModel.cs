using GalaSoft.MvvmLight;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace LangLang.ViewModel
{
    public class CourseListingViewModel : ViewModelBase
    {
        private ObservableCollection<CourseViewModel> _courses;
        private string _selectedLanguageName;
        private string _selectedLanguageLevel;
        private DateTime _selectedDate;
        private string _selectedDuration;
        private string _selectedFormat;
        public CourseListingViewModel()
        {
            _courses = new ObservableCollection<CourseViewModel>();
            CoursesCollectionView = CollectionViewSource.GetDefaultView(_courses);
            Language enga1 = new Language("English", LanguageLevel.A1);
            Language enga2 = new Language("English", LanguageLevel.A2);
            Language gera1 = new Language("German", LanguageLevel.A1);
            Language gera2 = new Language("German", LanguageLevel.A2);
            List<Language> peraLangs = new List<Language>
            {
                enga1,
                gera1
            };
            Teacher t1 = new("Pera", "Peric", "mijat2004@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Course course1 = new Course(new Language("en", LanguageLevel.B1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, t1.Id, new List<int> { 1 });
            Course course2 = new Course(new Language("en", LanguageLevel.B1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, t1.Id, new List<int> { 1 });
            Course course3 = new Course(new Language("en", LanguageLevel.B1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, t1.Id, new List<int> { 1 });
            Course course4 = new Course(new Language("en", LanguageLevel.B1), 5, new List<Weekday> { Weekday.Monday }, true, 1, 1, new TimeOnly(18), new DateOnly(2033, 4, 4), true, t1.Id, new List<int> { 1 });

            foreach (int courseId in Course.CourseIds)
            {
                _courses.Add(new CourseViewModel((Course)Course.GetById(courseId)));
            }

            CoursesCollectionView.Filter = filterCourses;
        }


        public ICollectionView CoursesCollectionView { get; }
        public IEnumerable<String> LanguageNameValues => Language.LanguageNames;
        public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        public IEnumerable<String> FormatValues => new List<String>{"online", "in-person"};
        public IEnumerable<CourseViewModel> Courses => _courses;

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

        private bool filterCourses(object obj)
        {
            if (obj is CourseViewModel courseViewModel)
            {
                return courseViewModel.FilterLanguageName(SelectedLanguageName) &&
                    courseViewModel.FilterLanguageLevel(SelectedLanguageLevel) &&
                    courseViewModel.FilterStartDate(SelectedDate) &&
                    courseViewModel.FilterDuration(SelectedDuration) &&
                    courseViewModel.FilterFormat(SelectedFormat); 
            }
            else
            {
                return false;
            }
        }

    }
}
