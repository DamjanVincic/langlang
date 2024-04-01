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
            // Define the collection and add three Language objects
            List<Language> languages = new List<Language>
            {
                new Language("Serbian", LanguageLevel.B2),
                new Language("English", LanguageLevel.A1),
                new Language("German", LanguageLevel.C1)
            };
            // Define a new list of ExamViewModel objects
            _exams = new ObservableCollection<ExamViewModel>
            {
                new ExamViewModel(new Exam(languages[0], 20, new DateOnly(2024, 4, 15))),
                new ExamViewModel(new Exam(languages[0], 20, new DateOnly(2024, 4, 15))),
                new ExamViewModel(new Exam(languages[0], 20, new DateOnly(2024, 4, 15))),
            };
            ExamCollectionView = CollectionViewSource.GetDefaultView(_exams);
            ExamCollectionView.Filter = FilterExams;
        }

        public IEnumerable<ExamViewModel> Exams => _exams;

        private bool FilterExams(object obj)
        {
            if (obj is ExamViewModel examViewModel)
            {
                return examViewModel.Language.Contains(ExamFilter) &&
                    examViewModel.LanguageLevel.Equals(ExamFilter) &&
                    examViewModel.ExamDate.Equals(ExamFilter);
            }
            return false; 
        }
    }
}
