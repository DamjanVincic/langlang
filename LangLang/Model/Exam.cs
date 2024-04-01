using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Exam : ScheduleItem
    {
        private static int _examId = 0;
        private static Dictionary<int, Exam> _exams = new Dictionary<int, Exam>();
        private Language _language;
        private int _maxStudents;
        private DateOnly _examDate;


        public Exam(Language language, int maxStudents, DateOnly examDate) : base(0, new TimeOnly())
        {
            throw new NotImplementedException();
            Language = language;
            MaxStudents = maxStudents;
            ExamDate = examDate;
            StudentIds = new List<int>();
            _examId++;
            Id = _examId;
            _exams.Add(Id, this);
        }

        public List<int> StudentIds { get; set; }

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
        public static Exam GetById(int id)
        {
            if (_exams.ContainsKey(id))
            {
                return _exams[id];
            }
            else
            {
                return null;
            }
        }
    }
}
