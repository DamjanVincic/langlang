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

        public EditTeacherViewModel(Teacher teacher)
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
        }

        private void Edit()
        {
            try
            {
                teacher.FirstName = FirstName;
                teacher.LastName = LastName;
                teacher.Email= Email;
                teacher.Password= Password;
                teacher.Phone= Phone;
                teacher.Gender = Gender;
                User pera=User.GetById(1);
                MessageBox.Show("User edited successfully.", "Success", MessageBoxButton.OK,
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
