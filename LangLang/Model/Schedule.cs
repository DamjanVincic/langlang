using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Schedule
    {
        public Dictionary<DateOnly, ScheduleItem> Table { get; set; }
    }
}
