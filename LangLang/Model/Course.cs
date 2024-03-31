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
        private int _id;
        private Language _language;
        private int _duration;
        private List<Weekday> _held;
        private bool _isOnline;
        private int _maxStudents;
        private int _creatorId;
        private TimeOnly _scheduledTime;
        private DateOnly _startDate;
        private bool _areApplicationsClosed;
        private int _teacherId;
        private List<int> _studentIds;
        public int Id {
            get 
            {
                return _id;
            }
            set 
            {
                _id = value;
            } 
        }
        public Language Language
        {
            get
            {
                return _language;
            }
            set
            {
                ValidateLanguage(value);
                _language = value;
            }
        }
        public int Duration 
        {
            get
            {
                return _duration;
            }
            set
            {
                ValidateDuration(value);
                _duration = value;
            }
        }
        public List<Weekday> Held {
            get
            {
                return _held;
            }
            set 
            {
                _held = value;
            }
        }
        public bool IsOnline {
            get
            {
                return _isOnline;
            }
            set 
            {
                _isOnline = value;
            } }
        public int MaxStudents 
        {
            get
            {
                return _maxStudents;
            }
            set
            {
                ValidateMaxStudents(value);
                _maxStudents = value;
            }
        }
        public int CreatorId {
            get
            {
                return _creatorId;
            }
            set
            {
                _creatorId = value;            
            } 
        }
        public TimeOnly ScheduledTime { 
            get 
            {
                return _scheduledTime;
            }
            set 
            {
                _scheduledTime = value;
            }
                }
        public DateOnly StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                ValidateStartDate(value);
                _startDate = value;
            }
        }
        public bool AreApplicationsClosed {
            get 
            {
                return _areApplicationsClosed;
            }
            set 
            { 
                _areApplicationsClosed = value;
            }
        }
        public int TeacherId {
            get 
            {
                return _teacherId;
            }
            set 
            {
                _teacherId = value;
            } 
        }
        public List<int> StudentIds { 
            get 
            {
                return _studentIds;
            } set
            {
                _studentIds = value;
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

        public void ValidateLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException("Language must not be null.");
            }
        }
    }
}
