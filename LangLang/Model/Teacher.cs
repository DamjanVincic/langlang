using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Teacher : User
    {
        public List<Language> Qualifications {  get; set; }
        public DateOnly DateCreated { get;}
        public List<Course> Courses { get; set; }


        public Teacher(string firstName, string lastName, string email, string password, Gender gender, string phone, List<Language> qualifications, List<Course> courses) : base(firstName, lastName, email, password, gender, phone)
        {
            Qualifications=qualifications;
            DateCreated=new DateOnly();
            Courses=courses;
        }
    }
}
