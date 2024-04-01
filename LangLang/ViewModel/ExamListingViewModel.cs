using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace LangLang.ViewModel
{
    public class ExamListingViewModel : ViewModelBase
    {
        ObservableCollection<ExamViewModel> _exams;

        private ExamViewModel _selectedItem;
        public ExamViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

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
            _deleteCommand = new RelayCommand(Delete);
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
        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(Delete));

        public void Delete()
        {
            try
            {
                if (SelectedItem != null)
                {
                    _exams.Remove((ExamViewModel)SelectedItem);
                    MessageBox.Show("Exam deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Please select an exam to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
