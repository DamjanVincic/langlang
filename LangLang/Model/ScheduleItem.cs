using System;

namespace LangLang.Model
{
    public abstract class ScheduleItem
    {
        protected ScheduleItem(int teacherId, TimeOnly scheduledTime)
        {
            TeacherId = teacherId;
            ScheduledTime = scheduledTime;
        }

        public int Id { get; set; }

        public int TeacherId { get; set; }

        public TimeOnly ScheduledTime { get; set; }
    }
}