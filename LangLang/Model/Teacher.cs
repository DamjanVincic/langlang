using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Teacher : User
    {
        private static List<int> _teacherIds = new();
        private List<int> _courseIds = new();
        private List<int> _examIds  = new();

        public static List<int> TeacherIds 
        {
            get => _teacherIds;
            set
            {
                _teacherIds = value;
            }
        }
        public List<Language> Qualifications { get; set; }
        public DateOnly DateCreated { get;}
        public List<int> CourseIds
        {
            get => _courseIds;
            set
            {
                _courseIds = value;
            }
        }
        public List<int> ExamIds
        {
            get => _examIds;
            set 
            {
                _examIds = value;
            }
        }

        public Teacher(string firstName, string lastName, string email, string password, Gender gender, string phone, List<Language> qualifications, List<int> courseIds) : base(firstName, lastName, email, password, gender, phone)
        {
            Qualifications=qualifications;
            DateCreated=DateOnly.FromDateTime(DateTime.Now);
            CourseIds=courseIds;
            if (TeacherIds==null)
            {
                TeacherIds=new List<int>();
            }
            TeacherIds.Add(Id);
        }
    }
}
