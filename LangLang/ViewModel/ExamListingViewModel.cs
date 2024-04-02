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
        private ObservableCollection<ExamViewModel> _exams;
        private Teacher _teacher;

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

        public ExamListingViewModel(Teacher teacher)
        {
            _teacher = teacher;
            _exams = new ObservableCollection<ExamViewModel>(Exam.GetTeacherExams(teacher.Id).Select(exam => new ExamViewModel(exam)));
            ExamCollectionView = CollectionViewSource.GetDefaultView(_exams);
            ExamCollectionView.Filter = FilterExams;
            DeleteCommand = new RelayCommand(Delete);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
        }

        public IEnumerable<ExamViewModel> Exams => _exams;

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
        public ICommand DeleteCommand { get; }
        public void Delete()
        {
            if (SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return;
                }
                
                ExamViewModel selectedExam = (ExamViewModel)SelectedItem;
                Exam exam = Exam.GetById(selectedExam.Id);

                DateTime todayDateTime = DateTime.Today;
                DateTime examDateTime = new DateTime(exam.ExamDate.Year, exam.ExamDate.Month, exam.ExamDate.Day);

                TimeSpan difference = examDateTime - todayDateTime;

                if (difference.TotalDays >= 14)
                {
                    Exam.GetById(SelectedItem.Id).Delete();
                    Teacher teacherOnExam = (Teacher)Teacher.GetUserById(exam.TeacherId);
                    teacherOnExam.ExamIds.Remove(exam.Id);
                    _exams.Remove(selectedExam);
                    ExamCollectionView.Refresh();
                    MessageBox.Show("Exam deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("You cannot cancel the exam as it is less than two weeks away.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No exam selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public ICommand AddCommand { get; }
        public void Add()
        {
            var newWindow = new AddExamView(_exams, ExamCollectionView, null,_teacher);
            newWindow.Show();
        }

        public ICommand EditCommand {  get; }
        public void Edit()
        {
            if (SelectedItem != null)
            {
                ExamViewModel selectedExam = (ExamViewModel)SelectedItem;
                Exam exam = Exam.GetById(selectedExam.Id);

                DateTime todayDateTime = DateTime.Today;
                DateTime examDateTime = new DateTime(exam.ExamDate.Year, exam.ExamDate.Month, exam.ExamDate.Day);

                TimeSpan difference = examDateTime - todayDateTime;

                if (difference.TotalDays >= 14)
                {
                    var newWindow = new AddExamView(_exams, ExamCollectionView, exam,_teacher);
                    newWindow.Show();
                }
                else
                {
                    MessageBox.Show("You cannot edit the exam as it is less than two weeks away.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an exam to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
