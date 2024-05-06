using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LangLang.Models
{
    public class Exam : ScheduleItem
    {
        public const int ExamDuration = 360;

        public Exam(Language language, int maxStudents, DateOnly date, int teacherId, TimeOnly time)
            : base(language, maxStudents, date, teacherId, time)
        {
        }

        // Constructor without date validation for deserializing
        [JsonConstructor]
        public Exam(int id, Language language, int maxStudents, DateOnly date, int teacherId, TimeOnly time)
            : base(id, language, maxStudents, date, teacherId, time)
        {
        }

        public new int MaxStudents
        {
            get => base.MaxStudents;
            set => base.MaxStudents = value;
        }

        public new DateOnly Date
        {
            get => base.Date;
            set
            {
                ValidateDate(value);
                base.Date = value;
            }
        }

        public List<int> StudentIds { get; set; } = new();

        private static void ValidateDate(DateOnly date)
        {
            if ((date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 14)
                throw new InvalidInputException("The exam has to be at least 2 weeks from now.");
        }

        public void RemoveStudent(int studentId)
        {
            if (!StudentIds.Contains(studentId))
                throw new InvalidInputException("Student hasn't applied to this exam.");

            StudentIds.Remove(studentId);
        }
    }
}