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
using LangLang.ViewModels.CourseViewModels;

namespace LangLang.ViewModels.StudentViewModels;

public class StudentCourseViewModel : ViewModelBase
{
    private readonly Student _student = UserService.LoggedInUser as Student ??
                                        throw new InvalidOperationException("No one is logged in.");

    private readonly ILanguageService _languageService = new LanguageService();
    private readonly IStudentService _studentService = new StudentService();

    private string? _selectedLanguageName;
    private string? _selectedLanguageLevel;
    private DateTime _selectedDate;
    private string _selectedDuration;
    private string _selectedFormat;

    private bool _applied;

    public StudentCourseViewModel(bool applied = false)
    {
        _applied = applied;
        
        AvailableCourses = new ObservableCollection<CourseViewModel>(
            (applied
                ? _studentService.GetAppliedCourses(_student.Id)
                : _studentService.GetAvailableCourses(_student.Id))
            .Select(course => new CourseViewModel(course)));
        CoursesCollectionView = CollectionViewSource.GetDefaultView(AvailableCourses);
        CoursesCollectionView.Filter = FilterCourses;

        ResetFiltersCommand = new RelayCommand(ResetFilters);
        ApplyForCourseCommand = new RelayCommand(ApplyForCourse);
        WithdrawFromCourseCommand = new RelayCommand(WithdrawFromCourse);
    }

    public ICommand ResetFiltersCommand { get; }
    public ICommand ApplyForCourseCommand { get; }
    public ICommand WithdrawFromCourseCommand { get; }

    public CourseViewModel? SelectedCourse { get; set; }
    public ObservableCollection<CourseViewModel> AvailableCourses { get; }
    public ICollectionView CoursesCollectionView { get; }
    public IEnumerable<string> LanguageNameValues => _languageService.GetAllNames();
    public IEnumerable<string> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
    public IEnumerable<string> FormatValues => new List<string> { "online", "in-person" };

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

    private void ApplyForCourse()
    {
        if (SelectedCourse == null)
            MessageBox.Show("Please select a course.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        try
        {
            _studentService.ApplyForCourse(_student.Id, SelectedCourse!.Id);
            MessageBox.Show("You have successfully applied for the course.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
            RefreshCourses(_applied);
        }
        catch (InvalidInputException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void WithdrawFromCourse()
    {
        if (SelectedCourse == null)
            MessageBox.Show("Please select a course.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        try
        {
            _studentService.WithdrawFromCourse(_student.Id, SelectedCourse!.Id);
            MessageBox.Show("You have successfully withdrawn from the course.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
            RefreshCourses(_applied);
        }
        catch (InvalidInputException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RefreshCourses(bool applied)
    {
        AvailableCourses.Clear();
        (applied ? _studentService.GetAppliedCourses(_student.Id) : _studentService.GetAvailableCourses(_student.Id))
            .ForEach(course => AvailableCourses.Add(new CourseViewModel(course)));
        CoursesCollectionView.Refresh();
    }
}