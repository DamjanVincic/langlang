using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.Model
{
    public class Course
    {
        public int Id { get; set; }
        public Language Language
        {
            get
            {
                return Language;
            }
            set
            {
                ValidateLanguage(value);
                Language = value;
            }
        }
        public int Duration 
        {
            get
            {
                return Duration;
            }
            set
            {
                ValidateDuration(value);
                Duration = value;
            }
        }
        public List<Weekday> Held { get; set; }
        public bool IsOnline {  get; set; }
        public int MaxStudents 
        {
            get
            {
                return MaxStudents;
            }
            set
            {
                ValidateMaxStudents(value);
                MaxStudents = value;
            }
        }
        public int CreatorId {  get; set; }
        public TimeOnly ScheduledTime {  get; set; }
        public DateOnly StartDate
        {
            get
            {
                return StartDate;
            }
            set
            {
                ValidateStartDate(value);
                StartDate = value;
            }
        }
        public bool AreApplicationsClosed {  get; set; }
        public int TeacherId {  get; set; }
        public List<int> StudentIds {  get; set; }

        public Course(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents, int creator, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed, int teacher, List<int> students)
        {
            ValidateLanguage(language);
            ValidateDuration(duration);
            ValidateMaxStudents(maxStudents);
            ValidateStartDate(startDate);
            Language = language;
            Duration = duration;
            Held = held;
            IsOnline = isOnline;
            MaxStudents = maxStudents;
            CreatorId = creator;
            ScheduledTime = scheduledTime;
            StartDate = startDate;
            AreApplicationsClosed = areApplicationsClosed;
            TeacherId = teacher;
            StudentIds = students;
        }

        private void ValidateStartDate(DateOnly startDate)
        {
            DateOnly today = new DateOnly();
            if (startDate < today)
            {
                throw new ArgumentNullException("Start date of course must be after today.");
            }
        }

        private void ValidateMaxStudents(int maxStudents)
        {
            if (maxStudents < 0)
            {
                throw new ArgumentNullException("Maximum number of students must not be negative.");
            }
        }

        //private void ValidateHeld(List<Weekday> held)
        //{
        //    if (held == null)
        //    {
        //        throw new ArgumentNullException("Held must not be null.");
        //    }
        //}

        private void ValidateDuration(int duration)
        {
            if (duration <= 0)
            {
                throw new ArgumentNullException("Duration must be positive.");
            }
        }

        private void ValidateLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException("Language must not be null.");
            }
        }

       
    }
    
}
