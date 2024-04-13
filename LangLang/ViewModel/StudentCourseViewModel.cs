using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.Services;

namespace LangLang.ViewModel;

public class StudentCourseViewModel : ViewModelBase
{
    private readonly ILanguageService _languageService = new LanguageService();
    private readonly IStudentService _studentService = new StudentService();
    
    private string _selectedLanguageName;
    private string _selectedLanguageLevel;
    private DateTime _selectedDate;
    private string _selectedDuration;
    private string _selectedFormat;
    
    public StudentCourseViewModel()
    {
        AvailableCourses = new ObservableCollection<CourseViewModel>(_studentService.GetAvailableCourses().Select(course => new CourseViewModel(course)));
        CoursesCollectionView = CollectionViewSource.GetDefaultView(AvailableCourses);
        CoursesCollectionView.Filter = FilterCourses;
        
        ResetFiltersCommand = new RelayCommand(ResetFilters);
    }

    public ICommand ResetFiltersCommand { get; }
    
    public ObservableCollection<CourseViewModel> AvailableCourses { get; }
    public ICollectionView CoursesCollectionView { get; }
    public IEnumerable<string> LanguageNameValues => _languageService.GetAllNames();
    public IEnumerable<string> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
    public IEnumerable<string> FormatValues => new List<string>{"online", "in-person"};

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
                courseViewModel.FilterFormat(SelectedFormat); 
        }

        return false;
    }
    
    private void ResetFilters()
    {
        SelectedLanguageLevel = null!;
        SelectedLanguageName = null!;
        SelectedDate = DateTime.MinValue;
        SelectedDuration = null!;
        SelectedFormat = null!;
    }
}