using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Exam
    {                 
        private int id;
        private Language language;
        private int maxStudents;
        private DateOnly examDate;
        public int Id
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
            }
        }
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
        public DateOnly ExamDate
        {
            get
            {
                return ExamDate;
            }
            set
            {
                ValidateExamDate(value);
                ExamDate = value;
            }
        }
        public List <int> studentIds { get; set; }

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
