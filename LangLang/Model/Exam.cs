using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Exam : Schedule
    {
        private static int _examId = 0;
        private static Dictionary<int, Exam> _exams = new Dictionary<int, Exam>();

        private Language _language;
        private int _maxStudents;
        private DateOnly _examDate;
        public List<int> StudentIds { get; set; }


        public Exam(Language language, int maxStudents, DateOnly examDate)
        {
            _examId++;
            Id = _examId;
            Language = language;
            MaxStudents = maxStudents;
            ExamDate = examDate;
            StudentIds = new List<int>();
        }

        public int Id { get; }

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

        public void ValidateMaxStudents(int maxStudents)
        {
            if (maxStudents < 0)
            {
                throw new InvalidInputException("Number of max students can not be negative.");
            }
        }

        public void ValidateLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
        }

        public void ValidateExamDate(DateOnly examDate)
        {
            DateOnly today = new DateOnly();
            if (examDate < today)
            {
                throw new ArgumentNullException("Date of exam must be after today.");
            }
        }
    }
}
