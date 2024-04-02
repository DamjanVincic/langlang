using LangLang.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Configuration;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace LangLang.ViewModel
{
    public class CourseViewModel : ViewModelBase
    {
        private readonly Course course;
        public int Id => course.Id;
        public string LanguageName => course.Language.Name;
        public LanguageLevel LanguageLevel => course.Language.Level;
        public string Duration => course.Duration + " weeks";
        public string Held => string.Join("/n", course.Held);
        public string IsOnline => course.IsOnline ? "online" : "in-person";
        public string Applications => course.AreApplicationsClosed ? "closed" : "opened";
        public int MaxStudents => course.MaxStudents;
        public TimeOnly ScheduledTime => course.ScheduledTime;
        public DateOnly StartDate => course.StartDate;
        public User Teacher => User.GetUserById(course.TeacherId);
        public string TeachersName => $"{Teacher.FirstName} {Teacher.LastName}";
        public string Students => string.Join("/n", course.StudentIds.Select(studentId => {
            User user = User.GetUserById(studentId);
            return user != null ? $"{user.FirstName} {user.LastName}" : "error";
        }));


        public ICommand AddCourseCommand { get; }
        public ICommand EditCourseCommand { get; }
        public ICommand DeleteCourseCommand { get; }

        public CourseViewModel(Course course)
        {
            this.course = course;
            //AddCourseCommand = new RelayCommand(AddCourse);
            //EditCourseCommand = new RelayCommand(EditCourse);
            //DeleteCourseCommand = new RelayCommand(DeleteCourse);
        }
        public bool FilterLanguageLevel(string languageLevel)
        {
            return languageLevel == null || course.Language.Level.ToString().Equals(languageLevel);
        }

        public bool FilterLanguageName(string languageName)
        {
            return languageName == null || course.Language.Name.Equals(languageName);
        }
        public bool FilterTeacher(int teacherId)
        {
            return teacherId == -1 || course.TeacherId == teacherId;
        }


        public bool FilterStartDate(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return true;
            } 
            DateOnly chosenDate = new DateOnly(date.Year, date.Month, date.Day);
            return chosenDate == course.StartDate;
        }
        public bool FilterDuration(string duration)
        {
            if (duration == null)
            {
                return true;
            }
            int.TryParse(duration.Split(" ")[0], out int result);
            return course.Duration == result;
        }

        public bool FilterFormat(string format)
        { 
            return format == null || format.Equals("online") == course.IsOnline;
        }
    }
}
