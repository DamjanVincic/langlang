using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;

namespace LangLang.ViewModel
{
    internal class AddTeacherViewModel:ViewModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public List<Language> Qualifications { get; set; }
        public ICommand AddLanguageCommand { get; }
        public ICommand AddTeacherCommand { get; }

        private ICollectionView teachersCollectionView;
        private ListBox qualificationsListBox;
        private ObservableCollection<TeacherViewModel> teachers;

        private Window _addTeachetWindow;
        
        public AddTeacherViewModel(ICollectionView teachersCollectionView,ListBox qualificationsListBox,ObservableCollection<TeacherViewModel> teachers, Window addTeacherWindow)
        {
            _addTeachetWindow = addTeacherWindow;
            
            AddLanguageCommand = new RelayCommand(OpenAddLanguageWindow);
            AddTeacherCommand = new RelayCommand(AddTeacher);
            this.teachersCollectionView= teachersCollectionView;
            this.QualificationCollectionView=CollectionViewSource.GetDefaultView(Language.Languages);
            this.qualificationsListBox = qualificationsListBox;
            this.teachers = teachers;
        }

        private void OpenAddLanguageWindow()
        {
            var newWindow = new AddLanguageView(QualificationCollectionView);

            newWindow.Show();
        }

        private void AddTeacher()
        {
            try
            {
                List<Language> languages = new List<Language>();
                foreach (var item in qualificationsListBox.SelectedItems)
                {
                    languages.Add((Language)item);
                }

                Teacher newTeacher = new Teacher(FirstName, LastName, Email, Password, Gender, Phone, languages);
                teachers.Add(new TeacherViewModel(newTeacher));
                teachersCollectionView.Refresh();
                MessageBox.Show("Teacher added successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _addTeachetWindow.Close();
            }
            catch (InvalidInputException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public ICollectionView QualificationCollectionView { get; set; }
        public IEnumerable<Gender> GenderValues => Enum.GetValues(typeof(Gender)).Cast<Gender>();
    }
}
