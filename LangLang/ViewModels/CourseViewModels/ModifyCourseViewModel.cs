using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;

namespace LangLang.ViewModels.CourseViewModels
{
    class ModifyCourseViewModel : ViewModelBase
    {
        private ILanguageService _languageService = new LanguageService();
        private ICourseService _courseService = new CourseService();

        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");

        private Course? _course;
        private ObservableCollection<CourseViewModel> _courses;
        private ICollectionView _courseCollectionView;

        private Window _modifyCourseWindow;

        public ModifyCourseViewModel(Course? course, Window modifyCourseWindow)
        {
            _modifyCourseWindow = modifyCourseWindow;

            _course = course;
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
                Format = course.IsOnline ? "online" : "in-person";
                CreatorId = course.CreatorId;
                ScheduledTime = course.ScheduledTime;
                Minutes = course.ScheduledTime.Minute;
                Hours = course.ScheduledTime.Hour;
                foreach (Weekday day in Held)
                {
                    switch ((int)day)
                    {
                        case 0:
                            IsMondayChecked = true;
                            break;
                        case 1:
                            IsTuesdayChecked = true;
                            break;
                        case 2:
                            IsWednesdayChecked = true;
                            break;
                        case 3:
                            IsThursdayChecked = true;
                            break;
                        case 4:
                            IsFridayChecked = true;
                            break;
                        case 5:
                            IsSaturdayChecked = true;
                            break;
                        case 6:
                            IsSundayChecked = true;
                            break;
                        default:
                            continue;
                    }
                }
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

        public bool MaxStudentsEnabled => Format != null && Format.Equals("in-person");
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

        public int Hours { get; set; }

        public int Minutes { get; set; }

        public List<string> SelectedWeekdays = new();


        public ICommand EnterCourseCommand { get; }

        public IEnumerable<LanguageLevel> LanguageLevelValues =>
            Enum.GetValues(typeof(LanguageLevel)).Cast<LanguageLevel>();

        public IEnumerable<string> LanguageNameValues => _languageService.GetAllNames();
        public IEnumerable<string> FormatValues => new List<string> { "online", "in-person" };
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
            if (string.IsNullOrEmpty(LanguageName) || LanguageLevel == null ||
                (!Format.Equals("online") && MaxStudents <= 0) || Duration <= 0 || StartDate == default || Hours < 0 ||
                Minutes < 0)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

                // TODO: Call course service to add or update the course, something similar to the comment below, refactor the rest
                
                /* try
                {
                    if (_course == null)
                    {
                        _courseService.Add(LanguageName, LanguageLevel, Duration, Held, isOnline, MaxStudents,
                            CreatorId, ScheduledTime, startDate, AreApplicationsClosed, _teacher.Id);
                        
                        MessageBox.Show("Course added successfully.", "Success", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        _courseService.Update(_course.Id, LanguageName, LanguageLevel, Duration, Held, isOnline,
                            MaxStudents, CreatorId, ScheduledTime, startDate, AreApplicationsClosed, _teacher.Id);
                        
                        MessageBox.Show("Exam added successfully.", "Success", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                catch (InvalidInputException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                } */

                // vv Old Code vv
                // if (Schedule.CanAddScheduleItem(startDate, Duration, Held, TeacherId, ScheduledTime, true, isOnline, _course))
                // {
                //     if (_course != null)
                //     {
                //         _course.Language.Name = LanguageName;
                //         _course.Language.Level = LanguageLevel;
                //         if (startDate != _course.StartDate)
                //         {
                //             Schedule.ModifySchedule(_course, _course.StartDate, _course.Duration, _course.Held, null);
                //             Schedule.ModifySchedule(_course, startDate, Duration, null, Held);
                //         }
                //         else if (!_course.Held.SequenceEqual(Held))
                //         {
                //             List<Weekday> removedDays = _course.Held.Except(Held).ToList();
                //             List<Weekday> addedDays = Held.Except(_course.Held).ToList();
                //             Schedule.ModifySchedule(_course, _course.StartDate, _course.Duration, (List<Weekday>)removedDays, null);
                //             Schedule.ModifySchedule(_course, _course.StartDate, Duration, null, (List<Weekday>)addedDays);
                //         }
                //
                //         
                //         _course.StartDate = startDate;
                //         _course.MaxStudents = MaxStudents;
                //         _course.ScheduledTime = ScheduledTime;
                //         _course.IsOnline = isOnline;
                //         _course.TeacherId = TeacherId;
                //         _course.Held = Held;
                //         _course.Duration = Duration;
                //         _course.CreatorId = CreatorId;
                //         _course.AreApplicationsClosed = AreApplicationsClosed;
                //         this._courseCollectionView.Refresh();
                //         MessageBox.Show("Course edited successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //     }
                //     else
                //     {
                //         // Course course = new(language, Duration, Held, isOnline, MaxStudents, _teacherId, ScheduledTime, startDate, AreApplicationsClosed, TeacherId, new List<int>());
                //         // _courseService.Add(language, Duration, Held, isOnline, MaxStudents, _teacherId, ScheduledTime, startDate, AreApplicationsClosed, TeacherId, new List<int>());
                //         // _courses.Add(new CourseViewModel(course));
                //         this._courseCollectionView.Refresh();
                //         MessageBox.Show("Course added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //     }
                //     _modifyCourseWindow.Close();
                // }
                // else
                // {
                //     MessageBox.Show("Unable to schedule the course. The selected date conflicts with an existing course schedule.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // }
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Language? IsValidLanguage(string languageName, LanguageLevel level)
        {
            foreach (Language language in _languageService.GetAll())
            {
                if (language.Name.Equals(languageName) && language.Level.Equals(level))
                {
                    return language;
                }
            }

            throw new InvalidInputException("Language doesn't exist.");
        }
    }
}