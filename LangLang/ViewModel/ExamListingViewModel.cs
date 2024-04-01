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
using LangLang.View;
using GalaSoft.MvvmLight;

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
                //OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICollectionView ExamCollectionView { get; set; }
        public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();


        // Add a property to access language names directly from the model
        public IEnumerable<string> LanguageNames => Language.LanguageNames;

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

        public ExamListingViewModel()
        {
            _exams = new ObservableCollection<ExamViewModel>
            {
                new ExamViewModel(new Exam(new Language("Serbian", LanguageLevel.B2), 20, new DateOnly(2024, 4, 15))),
                new ExamViewModel(new Exam(new Language("Serbian", LanguageLevel.B1), 30, new DateOnly(2024, 5, 20))),
                new ExamViewModel(new Exam(new Language("English", LanguageLevel.A1), 25, new DateOnly(2024, 6, 10))),
                new ExamViewModel(new Exam(new Language("English", LanguageLevel.A1), 35, new DateOnly(2024, 7, 5))),
                new ExamViewModel(new Exam(new Language("English", LanguageLevel.C2), 28, new DateOnly(2024, 8, 15)))
            };
            ExamCollectionView = CollectionViewSource.GetDefaultView(_exams);
            ExamCollectionView.Filter = FilterExams;
            DeleteCommand = new RelayCommand(Delete);
            AddCommand = new RelayCommand(Add);
            AddCommand = new RelayCommand(Edit);
        }

        public IEnumerable<ExamViewModel> Exams => _exams;

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
        public ICommand DeleteCommand { get; }
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
        public ICommand AddCommand { get; }
        public void Add()
        {
            var newWindow = new AddExamView();
            newWindow.Show();
            Application.Current.MainWindow.Closed += (sender, e) =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Application.Current.MainWindow)
                    {
                        window.Close();
                    }
                }
            };
        }

        public ICommand EditCommand {  get; }
        public void Edit()
        {
            if (SelectedItem != null)
            {
                var newWindow = new AddExamView(Exam.GetById(SelectedItem.Id));

                newWindow.Show();

            }
            else
            {
                MessageBox.Show("Please select an exam to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Application.Current.MainWindow.Closed += (sender, e) =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Application.Current.MainWindow)
                    {
                        window.Close();
                    }
                }
            };
        }

    }
}
