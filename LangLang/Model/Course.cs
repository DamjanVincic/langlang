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
        private int id;
        private Language language;
        private int duration;
        private List<Weekday> held;
        private bool isOnline;
        private int maxStudents;
        private int creatorId;
        private TimeOnly scheduledTime;
        private DateOnly startDate;
        private bool areApplicationsClosed;
        private int teacherId;
        private List<int> studentIds;
        public int Id {
            get 
            {
                return id;
            }
            set 
            {
                id = value;
            } 
        }
        public Language Language
        {
            get
            {
                return language;
            }
            set
            {
                ValidateLanguage(value);
                language = value;
            }
        }
        public int Duration 
        {
            get
            {
                return duration;
            }
            set
            {
                ValidateDuration(value);
                duration = value;
            }
        }
        public List<Weekday> Held {
            get
            {
                return held;
            }
            set 
            {
                held = value;
            }
        }
        public bool IsOnline {
            get
            {
                return isOnline;
            }
            set 
            {
                isOnline = value;
            } }
        public int MaxStudents 
        {
            get
            {
                return maxStudents;
            }
            set
            {
                ValidateMaxStudents(value);
                maxStudents = value;
            }
        }
        public int CreatorId {
            get
            {
                return creatorId;
            }
            set
            {
                creatorId = value;            
            } 
        }
        public TimeOnly ScheduledTime { 
            get 
            {
                return scheduledTime;
            }
            set 
            {
                scheduledTime = value;
            }
                }
        public DateOnly StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                ValidateStartDate(value);
                startDate = value;
            }
        }
        public bool AreApplicationsClosed {
            get 
            {
                return areApplicationsClosed;
            }
            set 
            { 
                areApplicationsClosed = value;
            }
        }
        public int TeacherId {
            get 
            {
                return teacherId;
            }
            set 
            {
                teacherId = value;
            } 
        }
        public List<int> StudentIds { 
            get 
            {
                return studentIds;
            } set
            {
                studentIds = value;
            }
        }

        public Course(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents, int creator, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed, int teacher, List<int> students)
        {
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
