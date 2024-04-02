using System;

namespace LangLang.Model
{
    public abstract class ScheduleItem
    {
        private int _teacherId;
        private TimeOnly _scheduledTime;
        public ScheduleItem(int teacherId, TimeOnly scheduledTime)
        {
           TeacherId = teacherId;
           ScheduledTime = scheduledTime;
        }

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
