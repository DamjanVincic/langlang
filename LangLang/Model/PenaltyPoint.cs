using System;
using Newtonsoft.Json;

namespace LangLang.Model
{
    public class PenaltyPoint
    {
        private PenaltyPointReason _penaltyPointReason;
        private bool _deleted;
        private int _studentId;
        private int _courseId;
        private int _teacherId;
        private DateOnly _datePenaltyPointGiven;

        public PenaltyPoint(PenaltyPointReason _penaltyPointReason, bool _deleted, int _studentId, int _courseId, int _teacherId, DateOnly _datePenaltyPointGiven)
        {
            PenaltyPointReason = _penaltyPointReason;   
            Deleted = _deleted;
            StudentId = _studentId;
            CourseId = _courseId;
            TeacherId = _teacherId;
            DatePenaltyPointGiven = _datePenaltyPointGiven;
        }

        [JsonConstructor]
        public PenaltyPoint(int id,PenaltyPointReason _penaltyPointReason, bool _deleted, int _studentId, int _courseId, int _teacherId, DateOnly _datePenaltyPointGiven)
        {
            Id = id;
            PenaltyPointReason = _penaltyPointReason;
            Deleted = _deleted;
            StudentId = _studentId;
            CourseId = _courseId;
            TeacherId = _teacherId;
            DatePenaltyPointGiven = _datePenaltyPointGiven;
        }

        public int Id { get; set; }
        public PenaltyPointReason PenaltyPointReason
        {
            get => _penaltyPointReason;
            set
            {
                _penaltyPointReason = value;
            }
        }

        public bool Deleted
        {
            get => _deleted;
            set
            {
                _deleted = value;
            }
        }

        public int StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
            }
        }

        public int CourseId
        {
            get => _courseId;
            set
            {
                _courseId = value;
            }
        }

        public int TeacherId
        {
            get => _teacherId;
            set
            {
                _teacherId = value;
            }
        }

        public DateOnly DatePenaltyPointGiven
        {
            get => _datePenaltyPointGiven;
            set
            {
                _datePenaltyPointGiven = value;
            }
        }
    }
}
