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

namespace LangLang.ViewModel;

public class StudentExamViewModel : ViewModelBase
{
    public ObservableCollection<ExamViewModel> AvailableExams { get; }

    public ExamViewModel SelectedItem { get; set; }

    public ICollectionView ExamCollectionView { get; set; }
    public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();
    
    public IEnumerable<string> LanguageNames => Language.LanguageNames;
    
    public ICommand ResetFiltersCommand { get; }
    
    public StudentExamViewModel()
    {
        AvailableExams = new ObservableCollection<ExamViewModel>(Exam.GetAvailableExams().Select(exam => new ExamViewModel(exam)));
        ExamCollectionView = CollectionViewSource.GetDefaultView(AvailableExams);
        ExamCollectionView.Filter = FilterExams;
        ResetFiltersCommand = new RelayCommand(ResetFilters);
    }

    private void ResetFilters()
    {
        LanguageNameSelected = null;
        LanguageLevelSelected = null;
        DateSelected = DateTime.MinValue;
    }

    private string _languageNameSelected;
    public string LanguageNameSelected
    {
        get => _languageNameSelected;
        set
        {
            _languageNameSelected = value;
            ExamCollectionView.Refresh();
        }
    }
    private string _languageLevelSelected;
    public string LanguageLevelSelected
    {
        get => _languageLevelSelected;
        set
        {
            _languageLevelSelected = value;
            ExamCollectionView.Refresh();
        }
    }
    private DateTime _dateSelected;
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
}