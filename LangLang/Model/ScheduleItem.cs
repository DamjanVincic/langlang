using System.Collections.Generic;

namespace LangLang.Model
{
    public abstract class ScheduleItem
    {
        private static Dictionary<int, ScheduleItem> _scheduleItems = new Dictionary<int, ScheduleItem>();
        private static int _idCounter = 1;
        public ScheduleItem()
        {
            Id = _idCounter++;
            _scheduleItems.Add(Id, this);
        }
        public int Id { get; set; }
        public static ScheduleItem GetById(int id)
        {
            return _scheduleItems[id];
        }
    }
}
