using System;
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
using LangLang.Views.ExamViews;

namespace LangLang.ViewModels.ExamViewModels
{
    public class ExamListingViewModel : ViewModelBase
    {
        private const int PageSize = 2;

        private readonly ITeacherService _teacherService = new TeacherService();
        private readonly ILanguageService _languageService = new LanguageService();
        private readonly IExamService _examService = new ExamService();

        private readonly ObservableCollection<ExamViewModel> _exams;

        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");

        private string? _languageNameSelected;
        private string? _languageLevelSelected;
        private DateTime _dateSelected;
        private int _currentPage = 1;

        public ExamListingViewModel()
        {
            _exams = new ObservableCollection<ExamViewModel>(_teacherService.GetExams(_teacher.Id)
                .Select(exam => new ExamViewModel(exam)));
            ExamCollectionView = CollectionViewSource.GetDefaultView(_exams);
            ExamCollectionView.Filter = FilterExams;
            ExamCollectionView.CollectionChanged += OnExamCollectionViewChanged;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);

            UpdateExamList();
        }

        public ExamViewModel? SelectedItem { get; set; }
        public ICollectionView ExamCollectionView { get; set; }

        public IEnumerable<LanguageLevel> LanguageLevelValues =>
            Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();

        public IEnumerable<string> LanguageNames => _languageService.GetAllNames();
        public IEnumerable<ExamViewModel> Exams => _exams;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand NextPageCommand => new RelayCommand(NextPage);
        public ICommand PreviousPageCommand => new RelayCommand(PreviousPage);

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
                       examViewModel.FilterDateHeld(DateSelected) &&
                       examViewModel.FilterTeacherId(_teacher.Id);
            }

            return false;
        }

        private void Add()
        {
            var newWindow = new AddExamView();
            newWindow.ShowDialog();
            UpdateExamList();
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
            UpdateExamList();
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
            UpdateExamList();

            MessageBox.Show("Exam deleted successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        /*
        private void UpdateExamList()
        {
            _exams.Clear();
            _teacherService.GetExams(_teacher.Id).ForEach(exam => _exams.Add(new ExamViewModel(exam)));
            ExamCollectionView.Refresh();
        }
        */

        private void NextPage()
        {
            _currentPage++;
            UpdateExamList();
        }

        private void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdateExamList();
            }
        }

        private void UpdateExamList()
        {
            _exams.Clear();
            var exams = _teacherService.GetExams(_teacher.Id)
                                       .Skip((_currentPage - 1) * PageSize)
                                       .Take(PageSize)
                                       .Select(exam => new ExamViewModel(exam));
            foreach (var exam in exams)
            {
                _exams.Add(exam);
            }
            ExamCollectionView.Refresh();
        }

        private void OnExamCollectionViewChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Exams));
        }
    }
}