﻿using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Course : ScheduleItem
    { 
        public const int CLASS_DURATION = 90; 
        private static int _idCounter = 1;
        private static Dictionary<int, Course> _courses = new Dictionary<int, Course>();
        private Language _language;
        private int _duration;
        private List<Weekday> _held;
        private int _maxStudents;
        private DateOnly _startDate;
        private static List<int> _courseIds = new List<int>();

        public Course(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents, int creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed, int teacherId, List<int> studentIds)
        {
            Language = language;
            Duration = duration;
            Held = held;
            CreatorId = creatorId;
            AreApplicationsClosed = areApplicationsClosed;
            IsOnline = isOnline;
            MaxStudents = maxStudents;
            ScheduledTime = scheduledTime;
            StartDate = startDate;
            TeacherId = teacherId;
            StudentIds = studentIds;
            Id = _idCounter++;
            _courses.Add(Id, this);
            CourseIds.Add(Id);
        }
        public int Id { get; set; }
        public static List<int> CourseIds 
        {
            get => _courseIds;
            set
            {
                _courseIds = value;
            }
        }

        public Language Language
        {
            get => _language;
            set
            {
                ValidateLanguage(value);
                _language = value;
            }
        }
        public int Duration 
        {
            get => _duration;
            set
            {
                ValidateDuration(value);
                _duration = value;
            }
        }
        public List<Weekday> Held {
            get => _held;
            set 
            {
                ValidateHeld(value);
                _held = value;
            }
        }
        public bool IsOnline { get; set; }
        public int MaxStudents 
        {
            get => _maxStudents;
            set
            {
                ValidateMaxStudents(value);
                _maxStudents = value;
            }
        }
        public int CreatorId { get; set;}
        public TimeOnly ScheduledTime { get; set; }
        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                ValidateStartDate(value);
                _startDate = value;
            }
        }
        public bool AreApplicationsClosed {get; set; }
        public int TeacherId {get; set; }
        public List<int> StudentIds { get; set; }

        

        private void ValidateStartDate(DateOnly startDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            if (startDate < today)
            { 
                throw new InvalidInputException("Start date of course must be after today.");
            }
        }

        private void ValidateMaxStudents(int maxStudents)
        {
            if (maxStudents < 0)
            {
                throw new InvalidInputException("Maximum number of students must not be negative.");
            }
        }

        private void ValidateHeld(List<Weekday> held)
        {
            if (held == null)
            {
                throw new ArgumentNullException(nameof(held));
            }
        }

        private void ValidateDuration(int duration)
        {
            if (duration <= 0)
            {
                throw new InvalidInputException("Duration must be positive.");
            }
        }

        private void ValidateLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
        }

        public static Course GetById(int id)
        {
            return _courses[id];
        }
    }
}
