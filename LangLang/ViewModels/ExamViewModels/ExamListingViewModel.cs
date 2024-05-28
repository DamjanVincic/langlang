﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using LangLang.Views.ExamViews;

namespace LangLang.ViewModels.ExamViewModels
{
    public class ExamListingViewModel : ViewModelBase
    {
        private readonly ITeacherService _teacherService = new TeacherService();
        private readonly ILanguageService _languageService = new LanguageService();
        private readonly IExamService _examService = new ExamService();

        private readonly ObservableCollection<ExamViewModel> _exams;

        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");

        private string? _languageNameSelected;
        private string? _languageLevelSelected;
        private DateTime _dateSelected;
        private string? _selectedSortingWay;
        private string? _selectedPropertyName;

        private int _currentPage;
        private readonly int _itemsPerPage = 5;
        private int _totalPages;
        private int _totalExams;
        public ExamListingViewModel()
        {
            _currentPage = 1;
            _totalExams = _teacherService.GetExamCount(_teacher.Id);
            CalculateTotalPages();
            _exams = new ObservableCollection<ExamViewModel>(_teacherService.GetExams(_teacher.Id, _currentPage, _itemsPerPage)
                .Select(exam => new ExamViewModel(exam)));
            ExamCollectionView = CollectionViewSource.GetDefaultView(_exams);
            ExamCollectionView.Filter = FilterExams;
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            NextPageCommand = new RelayCommand(NextPage);
        }

        public ExamViewModel? SelectedItem { get; set; }
        public ICollectionView ExamCollectionView { get; set; }

        public static IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));

        public IEnumerable<String> LanguageNames => _languageService.GetAllNames();
        public IEnumerable<ExamViewModel> Exams => _exams;
        public static IEnumerable<String> SortingWays => new List<String> { "ascending", "descending" };
        public static IEnumerable<String> PropertyNames => new List<String> { "Language", "LanguageLevel", "ExamDate" };

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

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

        public string? SelectedSortingWay
        {
            get => _selectedSortingWay;
            set
            {
                _selectedSortingWay = value;
                ApplySorting();
            }
        }

        public string? SelectedPropertyName
        {
            get => _selectedPropertyName;
            set
            {
                _selectedPropertyName = value;
                ApplySorting();
            }
        }

        private void ApplySorting()
        {
            if (_selectedPropertyName == null || _selectedSortingWay == null)
                return;

            ExamCollectionView.SortDescriptions.Clear();
            var sortDirection = _selectedSortingWay.Equals("ascending") ? ListSortDirection.Ascending : ListSortDirection.Descending;
            ExamCollectionView.SortDescriptions.Add(new SortDescription(_selectedPropertyName, sortDirection));
        }

        private void Add()
        {
            var newWindow = new AddExamView();
            newWindow.ShowDialog();
            _totalExams = _teacherService.GetExamCount(_teacher.Id);
            CalculateTotalPages();
            RefreshExams();
        }

        private void Edit()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select an exam to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Exam exam = _examService.GetById(SelectedItem.Id) ?? throw new InvalidOperationException("Exam not found.");

            new AddExamView(exam).ShowDialog();
            RefreshExams();
        }

        // TODO: MNOC 3
        private void Delete()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No exam selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            Exam exam = _examService.GetById(SelectedItem.Id) ?? throw new InvalidOperationException("Exam not found.");
            _examService.Delete(exam.Id);
            _totalExams--;
            CalculateTotalPages();
            RefreshExams();

            MessageBox.Show("Exam deleted successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void CalculateTotalPages()
        {
            _totalPages = (int)Math.Ceiling((double)_totalExams / _itemsPerPage);
        }

        private void NextPage()
        {
            if (_currentPage + 1 > _totalPages) { return; }
            _currentPage++;
            RefreshExams();
        }

        private void PreviousPage()
        {
            if (_currentPage < 2) { return; }
            _currentPage--;
            RefreshExams();
        }
        private bool FilterExams(object obj)
        {
            if (obj is ExamViewModel examViewModel)
            {
                return examViewModel.FilterLanguageName(LanguageNameSelected) &&
                       examViewModel.FilterLevel(LanguageLevelSelected) &&
                       examViewModel.FilterDateHeld(DateSelected) &&
                       examViewModel.FilterTeacherId(_teacher.Id);
            }

            return false;
        }

        private void RefreshExams()
        {
            _exams.Clear();
            _teacherService.GetExams(_teacher.Id, _currentPage, _itemsPerPage).ForEach(exam => _exams.Add(new ExamViewModel(exam)));
            ExamCollectionView.Refresh();
        }
    }
}