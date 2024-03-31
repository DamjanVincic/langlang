using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Exam
    {
        public int Id;
        private Language _language;
        private int _maxStudents;
        private DateOnly _examDate;

        public Exam(int id, Language language, int maxStudents, DateOnly examDate, List<int> studentIds)
        {
            Id = id;
            Language = language;
            MaxStudents = maxStudents;
            ExamDate = examDate;
            StudentIds = studentIds;
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
        public DateOnly ExamDate
        {
            get
            {
                return _examDate;
            }
            set
            {
                ValidateExamDate(value);
                _examDate = value;
            }
        }
        public List<int> StudentIds { get; set; }

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
                throw new ArgumentNullException("Language must not be null.");
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
