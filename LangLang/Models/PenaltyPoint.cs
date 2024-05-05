﻿using System;

namespace LangLang.Models
{
    public class PenaltyPoint
    {
        public int Id { get; set; }
        public PenaltyPointReason PenaltyPointReason { get; set; }
        public bool Deleted { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public DateOnly DatePenaltyPointGiven { get; set; }

        public PenaltyPoint(PenaltyPointReason penaltyPointReason, bool deleted, int studentId, int courseId, int teacherId, DateOnly datePenaltyPointGiven)
        {
            PenaltyPointReason = penaltyPointReason;
            Deleted = deleted;
            StudentId = studentId;
            CourseId = courseId;
            TeacherId = teacherId;
            DatePenaltyPointGiven = datePenaltyPointGiven;
        }
    }
}