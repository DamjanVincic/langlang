using System;
using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Repositories;

public interface IScheduleRepository
{
    public List<ScheduleItem> GetByDate(DateOnly date);
    public void Add(ScheduleItem item);
}