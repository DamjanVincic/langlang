using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository = new ScheduleFileRepository();
    
    public void Add(ScheduleItem scheduleItem)
    {
        if (!ValidateScheduleItem(scheduleItem))
            throw new InvalidInputException("Schedule overlaps with existing items.");
        switch (scheduleItem)
        {
            case Course course:
                List<int> dayDifferences = CalculateDateDifferences(course.Held);
                DateOnly startDate = scheduleItem.Date;

                for (int i = 0; i < course.Duration; ++i)
                {
                    foreach (int day in dayDifferences)
                    {
                        _scheduleRepository.Add(scheduleItem, startDate);
                        startDate = startDate.AddDays(day);
                    }
                }
                break;
            case Exam:
                _scheduleRepository.Add(scheduleItem, scheduleItem.Date);
                break;
        }
    }

    public void Update(ScheduleItem scheduleItem)
    {
        ValidateScheduleItem(scheduleItem, true);
        Delete(scheduleItem.Id);
        Add(scheduleItem);
    }

    public void Delete(int id)
    {
        _scheduleRepository.Delete(id);
    }

    private bool ValidateScheduleItem(ScheduleItem scheduleItem, bool toEdit = false)
    {
        switch (scheduleItem)
        {
            case Course course:
                DateOnly date = course.StartDate;
                for (int i = 0; i < course.Duration; ++i)
                {
                    foreach (int day in CalculateDateDifferences(course.Held))
                    {
                        if (!IsAvailable(scheduleItem, date, toEdit))
                            return false;
                        
                        date = date.AddDays(day);
                    }
                }
                break;
            case Exam exam:
                if (!IsAvailable(exam, exam.Date, toEdit))
                    return false;
                break;
        }
        return true;
    }

    private static List<int> CalculateDateDifferences(List<Weekday> held)
    {
        List<int> dayDifferences = new();
        foreach(Weekday day in held)
        {
            if (day == held[0]) continue;
            
            dayDifferences.Add((int)day - (int)held[0]);
        }
        dayDifferences.Add(7 - (int)held[^1] + (int)held[0]);
        return dayDifferences;
    }

    private bool IsAvailable(ScheduleItem scheduleItem, DateOnly date, bool toEdit)
    {
        List<ScheduleItem> scheduleItems = _scheduleRepository.GetByDate(date);

        if (!scheduleItems.Any())
            return true;

        TimeOnly startTime = scheduleItem.ScheduledTime;
        TimeOnly endTime = scheduleItem is Course ? startTime.AddMinutes(Course.ClassDuration) : startTime.AddMinutes(Exam.ExamDuration);
        int amountOverlapping = 0;

        foreach (ScheduleItem item in scheduleItems)
        {
            if (item.Id == scheduleItem.Id && toEdit) continue;
            if (item.IsOnline && item.TeacherId != scheduleItem.TeacherId) continue;

            TimeOnly startTimeCheck, endTimeCheck;
            startTimeCheck = item.ScheduledTime;
            endTimeCheck = scheduleItem is Course ? startTimeCheck.AddMinutes(Course.ClassDuration) : startTimeCheck.AddMinutes(Exam.ExamDuration);
            if (!DoPeriodsOverlap(startTime, endTime, startTimeCheck, endTimeCheck)) continue;
            if (item.IsOnline || (!item.IsOnline && ++amountOverlapping >= 2))
                return false;
        }
        return true;
    }

    private static bool DoPeriodsOverlap(TimeOnly startTime, TimeOnly endTime, TimeOnly startTimeCheck, TimeOnly endTimeCheck)
    {
        return !(startTime >= endTimeCheck || startTimeCheck >= endTime);
    }
}