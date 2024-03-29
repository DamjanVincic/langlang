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
        public List<int> CourseIds { get; set; }


        public Teacher(string firstName, string lastName, string email, string password, Gender gender, string phone, List<Language> qualifications, List<int> courseIds) : base(firstName, lastName, email, password, gender, phone)
        {
            Qualifications=qualifications;
            DateCreated=DateOnly.FromDateTime(DateTime.Now);
            CourseIds=courseIds;
        }
    }
}
