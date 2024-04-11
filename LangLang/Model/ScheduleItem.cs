using System;

namespace LangLang.Model
{
    public abstract class ScheduleItem
    {
        private Language _language = null!;
        private int _maxStudents;
        private DateOnly _date;

        protected ScheduleItem(Language language, int maxStudents, DateOnly date, int teacherId, TimeOnly time)
        {
            Language = language;
            TeacherId = teacherId;
            ScheduledTime = time;
        }

        public int Id { get; set; }
        
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
        
        public int TeacherId { get; set; }

        public DateOnly Date
        {
            get => _date;
            set
            {
                ValidateDate(value);
                _date = value;
            }
        }
        
        public TimeOnly ScheduledTime { get; set; }
        
        private static void ValidateLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
        }
        
        private static void ValidateMaxStudents(int maxStudents)
        {
            if (maxStudents < 0)
            {
                throw new InvalidInputException("Number of max students can not be negative.");
            }
        }
        
        private static void ValidateDate(DateOnly examDate)
        {
            if (examDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new InvalidInputException("Date must be after today.");
            }
        }
    }
}