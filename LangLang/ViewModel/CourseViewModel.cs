using LangLang.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Configuration;

namespace LangLang.ViewModel
{
    public class CourseViewModel : ViewModelBase
    {
        private readonly Course course;

        public string LanguageName => course.Language.Name;
        public LanguageLevel LanguageLevel => course.Language.Level;
        public string Duration => course.Duration + " weeks";
        public string Held => string.Join("/n", course.Held);
        public string IsOnline => course.IsOnline ? "online" : "in-person";
        public int MaxStudents => course.MaxStudents;
        public string ScheduledTime => course.ScheduledTime.ToString();
        public string StartDate => course.StartDate.ToString();
        public User Teacher => User.GetUserById(course.TeacherId);
        public string TeachersName => $"{Teacher.FirstName} {Teacher.LastName}";
        public string Students => string.Join("/n", course.StudentIds.Select(studentId => {
            User user = User.GetUserById(studentId);
            return user != null ? $"{user.FirstName} {user.LastName}" : "error";
        }));

        public CourseViewModel(Course course)
        {
            this.course = course;
        }


    }
}
