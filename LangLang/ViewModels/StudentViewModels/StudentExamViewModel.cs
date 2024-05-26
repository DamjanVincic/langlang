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
using LangLang.ViewModels.ExamViewModels;

namespace LangLang.ViewModels.StudentViewModels;

public class StudentExamViewModel : ViewModelBase
{
    private readonly ILanguageService _languageService = new LanguageService();
    private readonly IStudentService _studentService = new StudentService();
    private readonly IExamService _examService = new ExamService();
    private Student _student = UserService.LoggedInUser as Student ??
                              throw new InvalidOperationException("No one is logged in.");
    private string? _languageNameSelected;
    private string? _languageLevelSelected;
    private DateTime _dateSelected;
    
    public StudentExamViewModel()
    {
        AvailableExams = new ObservableCollection<ExamViewModel>(_studentService.GetAvailableExams(_student).Select(exam => new ExamViewModel(exam)));
        ExamCollectionView = CollectionViewSource.GetDefaultView(AvailableExams);
        ExamCollectionView.Filter = FilterExams;
        ResetFiltersCommand = new RelayCommand(ResetFilters);

        ApplyForExamCommand = new RelayCommand(Apply);
    }
    
    public ObservableCollection<ExamViewModel> AvailableExams { get; }
    public ExamViewModel? SelectedItem { get; set; }
    public ICollectionView ExamCollectionView { get; set; }
    public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();
    public IEnumerable<string> LanguageNames => _languageService.GetAllNames();
    public ICommand ResetFiltersCommand { get; }
    public ICommand ApplyForExamCommand { get; }


    public string? LanguageNameSelected
    {
        get => _languageNameSelected;
        set
        {
            _languageNameSelected = value;
            ExamCollectionView.Refresh();
        }
    }
    
    public string? LanguageLevelSelected
    {
        get => _languageLevelSelected;
        set
        {
            _languageLevelSelected = value;
            ExamCollectionView.Refresh();
        }
    }
    
    public DateTime DateSelected
    {
        get => _dateSelected;
        set
        {
            _dateSelected = value;
            ExamCollectionView.Refresh();
        }
    }
    
    private bool FilterExams(object obj)
    {
        if (obj is ExamViewModel examViewModel)
        {
            return examViewModel.FilterLanguageName(LanguageNameSelected) &&
                   examViewModel.FilterLevel(LanguageLevelSelected) &&
                   examViewModel.FilterDateHeld(DateSelected);
        }
        return false;
    }
    
    private void ResetFilters()
    {
        LanguageNameSelected = null!;
        LanguageLevelSelected = null!;
        DateSelected = DateTime.MinValue;
    }

    private void Apply()
    {
        if (SelectedItem == null)
        {
            MessageBox.Show("No exam selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            Exam exam = _examService.GetById(SelectedItem.Id)!;
            _studentService.ApplyStudentExam(_student, exam.Id);
            MessageBox.Show("You have applied for the exam.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception err)
        {
            MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}