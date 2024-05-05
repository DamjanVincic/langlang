using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using LangLang.Models;

namespace LangLang.ViewModels.StudentViewModels
{
    public class SingleStudentViewModel:ViewModelBase
    {
        private readonly Student _student;

        public SingleStudentViewModel(Student student)
        {
            _student = student;
        }

        public int Id => _student.Id;
        public string FirstName => _student.FirstName;
        public string LastName => _student.LastName;
        public string Email => _student.Email;
        public Gender Gender => _student.Gender;
        public string Phone => _student.Phone;
        public Education? Education => _student.Education;
    }
}
