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

public class AppliedExamListingViewModel : ViewModelBase
{
    private readonly ILanguageService _languageService = new LanguageService();
    private readonly IStudentService _studentService = new StudentService();
    private readonly IExamService _examService = new ExamService();
    private string? _languageNameSelected;
    private string? _languageLevelSelected;
    private DateTime _dateSelected;
    private readonly Student _student = UserService.LoggedInUser as Student ?? throw new InvalidInputException("No one is logged in.");

    public AppliedExamListingViewModel()
    {
        AppliedExams = new ObservableCollection<ExamViewModel>(_studentService.GetAppliedExams(_student).Select(exam => new ExamViewModel(exam)));
        ExamCollectionView = CollectionViewSource.GetDefaultView(AppliedExams);
        ExamCollectionView.Filter = FilterExams;
        ResetFiltersCommand = new RelayCommand(ResetFilters);

        DropExamCommand = new RelayCommand(Drop);
    }

    public ObservableCollection<ExamViewModel> AppliedExams { get; }
    public ExamViewModel? SelectedItem { get; set; }
    public ICollectionView ExamCollectionView { get; set; }
    public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();
    public IEnumerable<string> LanguageNames => _languageService.GetAllNames();
    public ICommand ResetFiltersCommand { get; }
    public ICommand DropExamCommand { get; }


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

    private void Drop()
    {
        if (SelectedItem == null)
        {
            MessageBox.Show("No exam selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        try
        {
            Exam exam = _examService.GetById(SelectedItem.Id)!;
            _studentService.DropExam(exam, _student);
            UpdateExamList();
            MessageBox.Show("Exam droped successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        } catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void UpdateExamList()
    {
        AppliedExams.Clear();
        _studentService.GetAppliedExams(_student).ForEach(exam => AppliedExams.Add(new ExamViewModel(exam)));
        ExamCollectionView.Refresh();
    }
}