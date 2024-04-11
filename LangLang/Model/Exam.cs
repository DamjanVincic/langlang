using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Exam : ScheduleItem
    {
        public const int ExamDuration = 360;

        public Exam(Language language, int maxStudents, DateOnly date, int teacherId, TimeOnly time) : base(language, maxStudents, date, teacherId, time)
        {
            MaxStudents = maxStudents;
            Date = date;
        }
        
        public new int MaxStudents
        {
            get => base.MaxStudents;
            set => base.MaxStudents = value;
        }
        
        public new DateOnly Date
        {
            get => base.Date;
            set => base.Date = value;
        }

        public List<int> StudentIds { get; set; } = new();
    }
}