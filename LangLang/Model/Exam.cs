using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LangLang.Model
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
            set => base.Date = value;
        }

        public List<int> StudentIds { get; set; } = new();
    }
}