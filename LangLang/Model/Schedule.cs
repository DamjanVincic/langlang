using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Schedule
    {
        public Dictionary<DateOnly, ScheduleItem> Table { get; set; }
    }
}
