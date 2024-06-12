using System;

namespace LangLang.Models
{
    public abstract class ScheduleItem
    {
        private Language _language = null!;
        private int _maxStudents;
        private DateOnly _date;

        private readonly bool _loadingFromDatabase;
        
        protected ScheduleItem()
        {
            // Only Entity Framework uses empty constructor
            _loadingFromDatabase = true;
        }

        protected ScheduleItem(Language language, int maxStudents, DateOnly date, int? teacherId, TimeOnly time)
        {
            Language = language;
            TeacherId = teacherId;
            ScheduledTime = time;
            MaxStudents = maxStudents;
            Date = date;
            Confirmed = false;
        }

        // Constructor without date validation for deserializing
        protected ScheduleItem(int id, Language language, int maxStudents, DateOnly date, int? teacherId, TimeOnly time)
        {
            Id = id;
            Language = language;
            TeacherId = teacherId;
            MaxStudents = maxStudents;
            _date = date;
            ScheduledTime = time;
        }

        public int Id { get; set; }

        public bool Confirmed { get; set; }

        public Language Language
        {
            get => _language;
            set
            {
                ValidateLanguage(value);
                _language = value;
            }
        }

        public int MaxStudents
        {
            get => _maxStudents;
            protected set
            {
                ValidateMaxStudents(value);
                _maxStudents = value;
            }
        }

        public int? TeacherId { get; set; }

        public DateOnly Date
        {
            get => _date;
            protected set
            {
                ValidateDate(value);
                _date = value;
            }
        }

        public TimeOnly ScheduledTime { get; set; }

        public bool IsOnline
        {
            get
            {
                if (this is Course course)
                    return course.IsOnline;
                
                return false;
            }
        }

        private static void ValidateLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));
        }

        private static void ValidateMaxStudents(int maxStudents)
        {
            if (maxStudents < 0)
                throw new InvalidInputException("Number of max students can not be negative.");
        }

        private void ValidateDate(DateOnly date)
        {
            if (_loadingFromDatabase) return;
            if (date < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidInputException("Date must be after today.");
        }
    }
}