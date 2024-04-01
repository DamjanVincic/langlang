using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace LangLang.ViewModel
{
    public class ExamListingViewModel : ViewModelBase
    {
        ObservableCollection<ExamViewModel> _exams;
        public ICollectionView ExamCollectionView { get; set; }
        public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();

        // Add a property to access language names directly from the model
        public IEnumerable<string> LanguageNames => Language.LanguageNames;

        private string _languageNameSelected;
        public string LanguageNameSelected
        {
            get
            {
                return _languageNameSelected;
            }
            set
            {
                _languageNameSelected = value;
                ExamCollectionView.Refresh();
            }
        }
        private string _languageLevelSelected;
        public string LanguageLevelSelected
        {
            get
            {
                return _languageLevelSelected;
            }
            set
            {
                _languageLevelSelected = value;
                ExamCollectionView.Refresh();
            }
        }
        private DateOnly _dateSelected;
        public DateOnly DateSelected
        {
            get
            {
                return _dateSelected;
            }
            set
            {
                _dateSelected = value;
                ExamCollectionView.Refresh();
            }
        }

        private string _examFilter = string.Empty;
        public string ExamFilter
        {
            get
            {
                return _examFilter;
            }
            set
            {
                _examFilter = value;
                OnPropertyChanged(nameof(ExamFilter));
                ExamCollectionView.Refresh(); // Refresh the collection view when filter changes
            }
        }

        public ExamListingViewModel()
        {
            List<Language> languages = new List<Language>
            {
                new Language("Serbian", LanguageLevel.B2),
                new Language("Serbian", LanguageLevel.B1),
                new Language("English", LanguageLevel.A1),
                new Language("German", LanguageLevel.C1)
            };

            _exams = new ObservableCollection<ExamViewModel>
            {
                new ExamViewModel(new Exam(languages[0], 20, new DateOnly(2024, 4, 15))),
                new ExamViewModel(new Exam(languages[1], 30, new DateOnly(2024, 5, 20))),
                new ExamViewModel(new Exam(languages[2], 25, new DateOnly(2024, 6, 10))),
                new ExamViewModel(new Exam(languages[0], 35, new DateOnly(2024, 7, 5))),
                new ExamViewModel(new Exam(languages[1], 28, new DateOnly(2024, 8, 15)))
            };
            ExamCollectionView = CollectionViewSource.GetDefaultView(_exams);
            ExamCollectionView.Filter = filterExams;
        }

        public IEnumerable<ExamViewModel> Exams => _exams;

        private bool filterExams(object obj)
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
}
