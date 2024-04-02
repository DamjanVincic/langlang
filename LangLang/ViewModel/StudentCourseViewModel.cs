using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using LangLang.Model;

namespace LangLang.ViewModel;

public class StudentCourseViewModel : ViewModelBase
{
    public ObservableCollection<CourseViewModel> AvailableCourses { get; }
    
    public StudentCourseViewModel()
    {
        AvailableCourses = new ObservableCollection<CourseViewModel>(Course.GetAvailableCourses().Select(course => new CourseViewModel(course)));
        CoursesCollectionView = CollectionViewSource.GetDefaultView(AvailableCourses);
        CoursesCollectionView.Filter = filterCourses;
    }
    
    private string _selectedLanguageName;
    private string _selectedLanguageLevel;
    private DateTime _selectedDate;
    private string _selectedDuration;
    private string _selectedFormat;
    
    public ICollectionView CoursesCollectionView { get; }
    public IEnumerable<String> LanguageNameValues => Language.LanguageNames;
    public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
    public IEnumerable<String> FormatValues => new List<String>{"online", "in-person"};
    private void AddCourse(){}
    private void EditCourse() { }
    private void DeleteCourse() { }


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