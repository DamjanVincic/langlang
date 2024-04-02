using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace LangLang.Model
{
    public class Schedule
    {
        public static Dictionary<DateOnly, List<ScheduleItem>> Table = new Dictionary<DateOnly, List<ScheduleItem>>();
        public static List<DateOnly> ScheduleItemDates = new List<DateOnly>();
        
        public static bool CanAddScheduleItem(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse, bool isOnline)
        {
            CheckInputValidability(date, duration, held, teacherId, startTime, isCourse);
            // Temp list of dates on which course can be held
            ScheduleItemDates = new();
            List<int> dateDifferences = CalculateDateDifferences(held);
            if (!isCourse)
            {
                duration = 1;
                held = new List<Weekday> {(Weekday)Enum.Parse(typeof(Weekday), date.DayOfWeek.ToString())};
            }
            while (duration > 0)
            {
                for (int i = 0; i < held.Count; ++i) 
                {
                    if (IsAvailable(date, teacherId, startTime, isCourse, isOnline))
                    {
                        ScheduleItemDates.Add(date);
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
        public Weekday ConvertToWeekday(string dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case "Monday":
                    return Weekday.Monday;
                case "Tuesday":
                    return Weekday.Tuesday;
                case "Wednesday":
                    return Weekday.Wednesday;
                case "Thursday":
                    return Weekday.Thursday;
                case "Friday":
                    return Weekday.Friday;
                case "Saturday":
                    return Weekday.Saturday;
                case "Sunday":
                    return Weekday.Sunday;
                default:
                    throw new ArgumentException("Invalid day of week string.");
            }
        }

        private static void CheckInputValidability(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse)
        {
            if (duration == 0)
            {
                MessageBox.Show("Invalid input: Duration must be greater than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TimeSpan difference = date.ToDateTime(TimeOnly.MinValue) - DateTime.Now;
            if (difference.TotalDays < 7)
            {
                MessageBox.Show("The course can be created no later than 7 days before the start", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (held == null)
            {
                MessageBox.Show("Required argument cannot be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        internal static void ModifySchedule(ScheduleItem item, DateOnly startDate, int duration, List<Weekday> toDelete, List<Weekday> toAdd)
        {
            if (toDelete != null)
            {
                DeleteItem(item, startDate, duration, toDelete);
            }
            if (toAdd != null)
            {
                AddItem(item, startDate, duration, toAdd);
            }
        }

        private static void AddItem(ScheduleItem item, DateOnly startDate, int duration, List<Weekday> toAdd)
        {
            List<int> dayDifferences = CalculateDateDifferences(toAdd);
            while (duration > 0)
            {
                for (int i = 0; i < toAdd.Count; ++i)
                {
                    ScheduleItemDates.Add(startDate);
                    startDate.AddDays(dayDifferences[i]);
                }
                duration--;
            }
            foreach (DateOnly courseDate in Schedule.ScheduleItemDates)
            {
                if (!Schedule.Table.ContainsKey(courseDate))
                {
                    Schedule.Table.Add(courseDate, new());
                }
                Schedule.Table[courseDate].Add(item);
            }
        }

        private static void DeleteItem(ScheduleItem item, DateOnly startDate, int duration, List<Weekday> toDelete)
        {
            List<int> dayDifferences = CalculateDateDifferences(toDelete);
            while (duration > 0)
            {
                for (int i = 0; i < toDelete.Count; ++i)
                {
                    ScheduleItemDates.Add(startDate);
                    startDate.AddDays(dayDifferences[i]);
                }
                duration--;
            }
            foreach (DateOnly courseDate in Schedule.ScheduleItemDates)
            {
                Schedule.Table[courseDate].Remove(item);
            }
        }
    }
}
