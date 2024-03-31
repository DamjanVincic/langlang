using LangLang.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ViewModel
{
    public class TeacherViewModel : ViewModelBase
    {
        private readonly Teacher teacher;

        public string FirstName => teacher.FirstName;
        public string LastName => teacher.LastName;
        public string Email => teacher.Email;
        public string Password => teacher.Password;
        public Gender Gender => teacher.Gender;
        public string Phone => teacher.Phone;

        private string qualifications;
        public string Qualifications {
            get { return qualifications; }
            set { qualifications = value; }
        }
        public string DateAdded => teacher.DateCreated.ToString();

        public event PropertyChangedEventHandler? PropertyChanged;

        public TeacherViewModel(Teacher teacher)
        {
            this.teacher=teacher;
            qualifications="";
            foreach(Language language in teacher.Qualifications)
            {
                qualifications+=language.ToString()+", ";
            }
            qualifications=qualifications.Remove(Qualifications.Length-2);
        }


    }
}
