using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;

namespace LangLang.Model
{
    public class PenaltyPoint
    {
        private PenaltyPointReason _penaltyPointReason;
        private bool _deleted;
        private int _studentId;
        private int _courseId;
        private DateOnly _datePenaltyPointGiven;

        public PenaltyPoint(PenaltyPointReason _penaltyPointReason, bool _deleted, int _studentId, int _courseId, DateOnly _datePenaltyPointGiven)
        {
            PenaltyPointReason = _penaltyPointReason;   
            Deleted = _deleted;
            StudentId = _studentId;
            CourseId = _courseId;
            DatePenaltyPointGiven = _datePenaltyPointGiven;
        }

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
