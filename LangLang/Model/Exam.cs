using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Exam : ScheduleItem
    {
        public const int ExamDuration = 360;
        
        private Language _language = null!;
        private int _maxStudents;
        private DateOnly _examDate;

        public Exam(Language language, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime) : base(teacherId, examTime)
        {
            Language = language;
            MaxStudents = maxStudents;
            ExamDate = examDate;
            StudentIds = new List<int>();
        }

        public List<int> StudentIds { get; set; }

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
            set
            {
                ValidateMaxStudents(value);
                _maxStudents = value;
            }
        }

        public DateOnly ExamDate
        {
            get => _examDate;
            set
            {
                ValidateExamDate(value);
                _examDate = value;
            }
        }

        private static void ValidateMaxStudents(int maxStudents)
        {
            if (maxStudents < 0)
            {
                throw new InvalidInputException("Number of max students can not be negative.");
            }
        }

        private static void ValidateLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
        }

        private static void ValidateExamDate(DateOnly examDate)
        {
            DateOnly today = new DateOnly();
            if (examDate < today)
            {
                throw new InvalidInputException("Date of exam must be after today.");
            }
        }
    }
}
