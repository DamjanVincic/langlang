using System;

namespace LangLang.Model
{
    public abstract class ScheduleItem
    {
        private static int _idCounter = 1;

        private int _teacherId;
        private TimeOnly _scheduledTime;
        public ScheduleItem(int teacherId, TimeOnly scheduledTime, int id = -1)
        {
            Id = id != -1 ? id : _idCounter++;
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
        
        public static int IdCounter { get => _idCounter; set => _idCounter = value; }
    }
}
