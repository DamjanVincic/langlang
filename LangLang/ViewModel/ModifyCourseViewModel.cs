using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace LangLang.ViewModel
{
    class ModifyCourseViewModel:ViewModelBase
    {
        private Course _course;
        public ModifyCourseViewModel(Course course)
        {
            Language.Languages.Add(new("eng", LanguageLevel.A1));
            Language.Languages.Add(new("eng", LanguageLevel.A2));
            Language.Languages.Add(new("eng", LanguageLevel.B1));
            Language.Languages.Add(new("eng", LanguageLevel.B2));
            this._course = course;
            if (course != null)
            {
                LanguageName = course.Language.Name;
                LanguageLevel = course.Language.Level;
                MaxStudents = course.MaxStudents;
                StartDate = new DateTime(course.StartDate.Year, course.StartDate.Month, course.StartDate.Day);
                Duration = course.Duration;
                Held = course.Held;
                CreatorId = course.CreatorId;
                AreApplicationsClosed = course.AreApplicationsClosed;
                Format = course.IsOnline?"online":"in-person";
                CreatorId = course.CreatorId;
                ScheduledTime = course.ScheduledTime;
            }

            EnterCourseCommand = new RelayCommand(AddCourse);
        }

        private bool _isMondayChecked;
        public bool IsMondayChecked
        {
            get => _isMondayChecked; 
            set { Set(ref _isMondayChecked, value); }
        }

        private bool _isTuesdayChecked;
        public bool IsTuesdayChecked
        {
            get => _isTuesdayChecked; 
            set { Set(ref _isTuesdayChecked, value); }
        }

        private bool _isWednesdayChecked;
        public bool IsWednesdayChecked
        {
            get => _isWednesdayChecked;
            set { Set(ref _isWednesdayChecked, value); }
        }

        private bool _isThursdayChecked;
        public bool IsThursdayChecked
        {
            get => _isThursdayChecked;
            set { Set(ref _isThursdayChecked, value); }
        }

        private bool _isFridayChecked;
        public bool IsFridayChecked
        {
            get => _isFridayChecked;
            set { Set(ref _isFridayChecked, value); }
        }

        private bool _isSaturdayChecked;
        public bool IsSaturdayChecked
        {
            get => _isSaturdayChecked;
            set { Set(ref _isSaturdayChecked, value); }
        }

        private bool _isSundayChecked;
        public bool IsSundayChecked
        {
            get => _isSundayChecked;
            set { Set(ref _isSundayChecked, value); }
        }


        public string LanguageName { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public int MaxStudents { get; set; }
        public DateTime StartDate { get; set; }
        public string Format { get; set; }
        public bool AreApplicationsClosed { get; set; }
        public int Duration { get; set; }
        public TimeOnly ScheduledTime { get; set; }
        public List<Weekday> Held { get; set; }
        public int CreatorId { get; set; }
        public int TeacherId { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public List<string> SelectedWeekdays = new();


        public ICommand EnterCourseCommand { get; }

        public IEnumerable<LanguageLevel> LanguageLevelValues => Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();
        public IEnumerable<string> LanguageNameValues => Language.LanguageNames;
        public IEnumerable<string> FormatValues => new List<string> { "online", "in-person"};
        public IEnumerable<Weekday> WeekdayValues => Enum.GetValues(typeof(Weekday)).Cast<Weekday>();
        readonly List<string> hours = CreateHourValues();
        private static List<string> CreateHourValues()
        {
            List<string> hours = new();
            for (int hour = 0; hour < 24; hour++)
            {
                hours.Add(hour.ToString("00"));
            }
            return hours;
        }
        readonly List<string> minutes = CreateMinuteValues();

        private static List<string> CreateMinuteValues()
        {
            List<string> minutes = new();
            for (int minute = 0; minute < 60; minute += 15)
            {
                minutes.Add(minute.ToString("00"));
            }
            return minutes;
        }
        public IEnumerable<string> HourValues => hours;
        public IEnumerable<string> MinuteValues => minutes;


        private void AddCourse()
        {
            try
            {
                List<Weekday> selectedWeekdays = new List<Weekday>();
                if (IsMondayChecked)
                    selectedWeekdays.Add(Weekday.Monday);
                if (IsTuesdayChecked)
                    selectedWeekdays.Add(Weekday.Tuesday);
                if (IsWednesdayChecked)
                    selectedWeekdays.Add(Weekday.Wednesday);
                if (IsThursdayChecked)
                    selectedWeekdays.Add(Weekday.Thursday);
                if (IsFridayChecked)
                    selectedWeekdays.Add(Weekday.Friday);
                if (IsSaturdayChecked)
                    selectedWeekdays.Add(Weekday.Saturday);
                if (IsSundayChecked)
                    selectedWeekdays.Add(Weekday.Sunday);
                Held = selectedWeekdays;
                Language? language = IsValidLanguage(LanguageName, LanguageLevel);
                ScheduledTime = new TimeOnly(Hours * 60 + Minutes);
                bool isOnline = Format.Equals("online") ? true : false;
                DateOnly startDate = new DateOnly(StartDate.Year, StartDate.Month, StartDate.Day);
                if (CanAddScheduleItem(startDate, Duration, Held, TeacherId, ScheduledTime, true, isOnline) && !language.Equals(null))
                {
                    if (_course != null)
                    {
                        _course.Language.Name = LanguageName;
                        _course.Language.Level = LanguageLevel;
                        if (startDate != _course.StartDate)
                        {
                            Schedule.ModifySchedule(_course, _course.StartDate, _course.Duration, _course.Held, null);
                            Schedule.ModifySchedule(_course, startDate, Duration, null, Held);
                        }
                        else if (!_course.Held.SequenceEqual(Held))
                        {
                            IEnumerable<Weekday> removedDays = _course.Held.Except(Held);
                            IEnumerable<Weekday> addedDays = Held.Except(_course.Held);
                            Schedule.ModifySchedule(_course, _course.StartDate, _course.Duration, (List<Weekday>)removedDays, null);
                            Schedule.ModifySchedule(_course, _course.StartDate, Duration, null, (List<Weekday>)addedDays);
                        }

                        _course.StartDate = startDate;
                        _course.MaxStudents = MaxStudents;
                        _course.ScheduledTime = ScheduledTime;
                        _course.IsOnline = isOnline;
                        _course.TeacherId = TeacherId;
                        _course.Held = Held;
                        _course.Duration = Duration;
                        _course.CreatorId = CreatorId;
                        _course.AreApplicationsClosed = AreApplicationsClosed;
                    }
                    else
                    {
                        Course course = new(language, Duration, Held, isOnline, MaxStudents, CreatorId, ScheduledTime, startDate, AreApplicationsClosed, TeacherId, new List<int>());
                        foreach (DateOnly courseDate in Schedule.CourseDates)
                        {
                            Schedule.Table[courseDate].Add(course);
                        }
                    }
                    MessageBox.Show("Exam added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Unable to schedule the course. The selected date conflicts with an existing course schedule.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool CanAddScheduleItem(DateOnly date, int duration, List<Weekday> held, int teacherId, TimeOnly startTime, bool isCourse, bool isOnline)
        {
            return true;
        }
        public Language? IsValidLanguage(string languageName, LanguageLevel level)
        {
            foreach (Language language in Language.Languages)
            {
                if (language.Name.Equals(languageName) && language.Level.Equals(level))
                {
                    return language;
                }
            }
            return null;
        }

    }
    
}

