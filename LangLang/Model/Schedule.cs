using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;

namespace LangLang.Model
{
    public class Schedule
    {
        public static Dictionary<DateOnly, List<ScheduleItem>> Table { get; set; }
        public static List<DateOnly> CourseDates {get; set; }
        
        public static bool CanAddScheduleItem(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse, bool isOnline)
        {
            CheckInputValidability(date, duration, held, teacherId, startTime, isCourse);
            CourseDates = new();
            List<int> dateDifferences = CalculateDateDifferences(held);
            while (duration > 0)
            {
                for (int i = 0; i < held.Count; ++i) 
                {
                    if (IsAvailable(date, teacherId, startTime, isCourse, isOnline))
                    {
                        CourseDates.Add(date);
                        date.AddDays(dateDifferences[i]);
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

        private static void CheckInputValidability(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse)
        {
            if (duration == 0)
            {
                throw new ArgumentException("Invalid input: Duration must be greater than 0.", nameof(duration));
            }
            TimeSpan difference = date.ToDateTime(TimeOnly.MinValue) - DateTime.Now;
            if (difference.TotalDays < 7)
            {
                throw new ArgumentException("The course can be created no later than 7 days before the start", nameof(date));
            }
            if (held == null)
            {
                throw new ArgumentNullException(nameof(held), "Required argument cannot be null.");
            }
        }

        private static List<int> CalculateDateDifferences(List<Weekday> held)
        {
            List<int> dayDifferences = new();
            foreach(Weekday day in held)
            {
                dayDifferences.Add((int)day - (int)held[0]);
            }
            dayDifferences.Add(7 - (int)held[^1]);
            return dayDifferences;
        }

        private static bool IsAvailable(DateOnly date, int teacherId, TimeOnly startTime, bool isCourse, bool isOnline)
        {
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
                    if ((item.TeacherId == teacherId))
                    {
                        return false;
                    }
                    else if (!isOnline)
                    {
                        ++overlaps;
                        if (overlaps >= 2)
                        {
                            return false;
                        }
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
    }
}
