using System;
using System.Collections.Generic;
using System.Windows;

namespace LangLang.Model
{
    public class Schedule
    {
        public static Dictionary<DateOnly, List<ScheduleItem>> Table = new Dictionary<DateOnly, List<ScheduleItem>>();
        public static List<DateOnly> ScheduleItemDates = new List<DateOnly>();
        
        public static bool CanAddScheduleItem(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse, bool isOnline, Course course = null)
        {
            // Temp list of dates on which course can be held
            ScheduleItemDates = new();
            List<int> dateDifferences = CalculateDateDifferences(held);
            
            while (duration > 0)
            {
                for (int i = 0; i < held.Count; ++i) 
                {
                    if (IsAvailable(date, teacherId, startTime, isCourse, isOnline, course))
                    {
                        ScheduleItemDates.Add(date);
                        date = date.AddDays(dateDifferences[i]);
                    }
                    else
                    {
                        return false;
                    }
                }
                duration--;
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

        private static bool IsAvailable(DateOnly date, int teacherId, TimeOnly startTime, bool isCourse, bool isOnline, Course course = null)
        {
            if (!Table.ContainsKey(date))
            {
                return true;
            }
            List<ScheduleItem> scheduleItems = Table[date];
            TimeOnly endTime;
            TimeOnly startTimeCheck;
            TimeOnly endTimeCheck;
            int overlaps = 0;
            if (isCourse == true)
            {
                endTime = startTime.AddMinutes(90);
            }
            else 
            { 
                endTime = startTime.AddHours(4);
            }
            foreach (ScheduleItem item in scheduleItems)
            {
                startTimeCheck = item.ScheduledTime;
                if (isCourse == true)
                {
                    endTimeCheck = startTimeCheck.AddMinutes(90);
                }
                else
                {
                    endTimeCheck = startTimeCheck.AddHours(4);
                }
                if (!DoPeriodsOverlap(startTime, endTime, startTimeCheck, endTimeCheck))
                {
                    continue;
                }
                else
                {
                    if (course == null && item.TeacherId == teacherId)
                    {
                        return false;
                    }
                    
                    if (!isOnline)
                    {
                        ++overlaps;
                        if (overlaps >= 2)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (course == null)
                            return false;
                    }
                }
            }
            return true;
        }

        private static bool DoPeriodsOverlap(TimeOnly startTime, TimeOnly endTime, TimeOnly startTimeCheck, TimeOnly endTimeCheck)
        {
            if (startTime > startTimeCheck)
            {
                if (startTime >= endTimeCheck)
                {
                    return false;
                }
                else 
                {
                    return true; 
                }
            }
            else if (startTimeCheck > startTime)
            {
                if (startTimeCheck >= endTime)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        internal static void ModifySchedule(ScheduleItem item, DateOnly startDate, int duration, List<Weekday> toDelete, List<Weekday> toAdd)
        {
            if (toDelete != null && toDelete.Count != 0)
            {
                DeleteItem(item, startDate, duration, toDelete);
            }
            if (toAdd != null && toAdd.Count != 0)
            {
                AddItem(item, startDate, duration, toAdd);
            }
        }

        private static void DeleteItem(ScheduleItem item, DateOnly startDate, int duration, List<Weekday> toDelete)
        {
            ScheduleItemDates = new List<DateOnly>();
            List<int> dayDifferences = CalculateDateDifferences(toDelete);
            while (duration > 0)
            {
                for (int i = 0; i < toDelete.Count; ++i)
                {
                    ScheduleItemDates.Add(startDate);
                    startDate = startDate.AddDays(dayDifferences[i]);
                }
                duration--;
            }
            foreach (DateOnly courseDate in Schedule.ScheduleItemDates)
            {
                if (!Schedule.Table.ContainsKey(courseDate))
                {
                    MessageBox.Show("The schedule item has already been deleted", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Schedule.Table[courseDate].Remove(item);
                }
            }
        }
        
        private static void AddItem(ScheduleItem item, DateOnly startDate, int duration, List<Weekday> toAdd)
        {
            ScheduleItemDates = new List<DateOnly>();
            List<int> dayDifferences = CalculateDateDifferences(toAdd);
            while (duration > 0)
            {
                for (int i = 0; i < toAdd.Count; ++i)
                {
                    ScheduleItemDates.Add(startDate);
                    startDate = startDate.AddDays(dayDifferences[i]);
                }
                duration--;
            }
            foreach (DateOnly courseDate in Schedule.ScheduleItemDates)
            {
                if (!Schedule.Table.ContainsKey(courseDate))
                {
                    Schedule.Table.Add(courseDate, new List<ScheduleItem>());
                }
                Schedule.Table[courseDate].Add(item);
            }
        }
    }
}

