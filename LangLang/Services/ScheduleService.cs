using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository = new ScheduleFileRepository();
    
    public void Add(ScheduleItem item)
    {
        if (!ValidateScheduleItem(item))
            throw new InvalidInputException("Schedule overlaps with existing items.");
        
        switch (item)
        {
            case Course course:
                List<int> dayDifferences = CalculateDateDifferences(course.Held);
                DateOnly startDate = item.Date;

                for (int i = 0; i < course.Duration; ++i)
                {
                    foreach (int day in dayDifferences)
                    {
                        // No need to change the date if we add it to Add method as a parameter
                        course.StartDate = startDate;
                        _scheduleRepository.Add(item);
                        startDate = startDate.AddDays(day);
                    }
                }
                break;
            case Exam:
                _scheduleRepository.Add(item);
                break;
        }
    }

    private bool ValidateScheduleItem(ScheduleItem scheduleItem)
    {
        switch (scheduleItem)
        {
            case Course course:
                DateOnly date = course.StartDate;
                for (int i = 0; i < course.Duration; ++i)
                {
                    foreach (int day in CalculateDateDifferences(course.Held))
                    {
                        if (!IsAvailable(scheduleItem, date))
                            return false;
                        
                        date = date.AddDays(day);
                    }
                }
                break;
            case Exam exam:
                if (!IsAvailable(exam, exam.Date))
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
            if (day == held[0])
                continue;
            
            dayDifferences.Add((int)day - (int)held[0]);
        }
        dayDifferences.Add(7 - (int)held[^1] + (int)held[0]);
        return dayDifferences;
    }

    private bool IsAvailable(ScheduleItem scheduleItem, DateOnly date, bool toEdit = false)
    {
        // TODO: Refactor this method
        
        List<ScheduleItem> scheduleItems = _scheduleRepository.GetByDate(date);

        if (!scheduleItems.Any())
            return true;

        TimeOnly startTime = scheduleItem.ScheduledTime;
        TimeOnly endTime = scheduleItem is Course ? startTime.AddMinutes(Course.ClassDuration) : startTime.AddMinutes(Exam.ExamDuration);
        // The amount of overlapping in person classes
        int amountOverlapping = 0;
        foreach (ScheduleItem item in scheduleItems)
        {
            // If it's the same item, skip it
            if (item.Id == scheduleItem.Id && toEdit) continue;

            TimeOnly startTimeCheck, endTimeCheck;
            if (scheduleItem.IsOnline)
            {
                if (item.TeacherId != scheduleItem.TeacherId) continue;
                
                startTimeCheck = item.ScheduledTime;
                endTimeCheck = scheduleItem is Course ? startTimeCheck.AddMinutes(Course.ClassDuration) : startTimeCheck.AddMinutes(Exam.ExamDuration);

                if (!DoPeriodsOverlap(startTime, endTime, startTimeCheck, endTimeCheck)) continue;

                return false;
            }
            
            // If it's offline, it also can't overlap with two offline items, doesn't matter if it's the same teacher
            
            startTimeCheck = item.ScheduledTime;
            endTimeCheck = scheduleItem is Course ? startTimeCheck.AddMinutes(Course.ClassDuration) : startTimeCheck.AddMinutes(Exam.ExamDuration);
            
            if (!DoPeriodsOverlap(startTime, endTime, startTimeCheck, endTimeCheck)) continue;

            if (scheduleItem.TeacherId == item.TeacherId)
                return false;

            if (!item.IsOnline && ++amountOverlapping >= 2)
                return false;
        }
        return true;
    }

    private static bool DoPeriodsOverlap(TimeOnly startTime, TimeOnly endTime, TimeOnly startTimeCheck, TimeOnly endTimeCheck)
    {
        return !(startTime >= endTimeCheck || startTimeCheck >= endTime);
    }
}