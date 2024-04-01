using GalaSoft.MvvmLight;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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

        public string Qualifications => string.Join(", ", teacher.Qualifications);

        public string DateAdded => teacher.DateCreated.ToString();

        public event PropertyChangedEventHandler? PropertyChanged;

        public TeacherViewModel(Teacher teacher)
        {
            this.teacher=teacher;
        }

        public bool FilterLanguageLevel(string languageLevel)
        {
            return languageLevel==null || teacher.Qualifications.Where(language => language.Level == (LanguageLevel)Enum.Parse(typeof(LanguageLevel), languageLevel)).Count()!=0;
        }

        public bool FilterLanguageName(string languageName)
        {
            return languageName==null || teacher.Qualifications.Where(language => language.Name.Equals(languageName)).Count()!=0;
        }

        public bool FilterDateCreated(DateTimeOffset dateCreated)
        {
            return dateCreated==DateTimeOffset.MinValue || teacher.DateCreated==DateOnly.FromDateTime(dateCreated.Date);
        }
    }
}
