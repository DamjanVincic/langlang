using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight;
using LangLang.Model;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

namespace LangLang.ViewModel
{
    internal class EditTeacherViewModel:ViewModelBase
    {
        private Teacher teacher;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }

        public List<string> Qualifications { get; set; }

        public ICommand SaveEditCommand { get; }
        private ICollectionView teachersCollectionView;

        public EditTeacherViewModel(Teacher teacher, ICollectionView teachersCollectionView)
        {
            this.teacher=teacher;
            FirstName = this.teacher.FirstName;
            LastName = this.teacher.LastName;
            Email = this.teacher.Email;
            Password = this.teacher.Password;
            Gender = this.teacher.Gender;
            Phone = this.teacher.Phone;
            Qualifications = this.teacher.Qualifications.ConvertAll(qualification => qualification.ToString()); ;
            SaveEditCommand = new RelayCommand(Edit);
            this.teachersCollectionView = teachersCollectionView;
        }

        private void Edit()
        {
            try
            {
                teacher.Edit(FirstName,LastName,Email,Password,Gender,Phone);
                teachersCollectionView.Refresh();
                MessageBox.Show("Teacher edited successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
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

        public IEnumerable<Gender> GenderValues => Enum.GetValues(typeof(Gender)).Cast<Gender>();

        public IEnumerable<Education> EducationValues => Enum.GetValues(typeof(Education)).Cast<Education>();
    }
}
