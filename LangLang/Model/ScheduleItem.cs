using System;

namespace LangLang.Model
{
    public abstract class ScheduleItem
    {
        private static int _idCounter = 1;

        private int _teacherId;
        private TimeOnly _scheduledTime;
        public ScheduleItem(int teacherId, TimeOnly scheduledTime)
        {
            Id = _idCounter;
            _idCounter++;
            TeacherId = teacherId;
            ScheduledTime = scheduledTime;
        }
        public int Id { get; set; }

        public int TeacherId
        { 
            get => _teacherId; 
            set =>_teacherId = value;
        }

        public TimeOnly ScheduledTime
        {
            get => _scheduledTime;
            set => _scheduledTime = value; 
        }
    }
}
