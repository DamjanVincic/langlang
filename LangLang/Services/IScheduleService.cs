using LangLang.Model;
using System;

namespace LangLang.Services;

public interface IScheduleService
{
    // TODO: Change the way items are scheduled, if they're scheduled on one date, but not that weekday etc.
    public void Add(ScheduleItem scheduleItem);
    public void Update(ScheduleItem scheduleItem);
    public void Delete(int id);
    public bool ValidateScheduleItem(ScheduleItem scheduleItem, bool toEdit = false);
}