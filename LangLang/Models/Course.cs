﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LangLang.Models
{
    public class Course : ScheduleItem
    {
        public const int ClassDuration = 90;

        private int _duration;
        private List<Weekday> _held = null!;

        public Course(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents,
            int? creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed,
            int teacherId) : base(language, maxStudents, startDate, teacherId, scheduledTime)
        {
            Duration = duration;
            StartDate = startDate;
            Held = held;
            CreatorId = creatorId;
            AreApplicationsClosed = areApplicationsClosed;
            IsOnline = isOnline;
        }

        // Constructor without date validation for deserializing
        [JsonConstructor]
        public Course(int id, Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents,
            bool isFinished, bool studentsNotified, int creatorId, TimeOnly scheduledTime, DateOnly startDate,
            bool areApplicationsClosed, int teacherId, Dictionary<int, ApplicationStatus> students)
                        : base(id, language, maxStudents, startDate, teacherId, scheduledTime)
        {
            Duration = duration;
            Held = held;
            CreatorId = creatorId;
            AreApplicationsClosed = areApplicationsClosed;
            IsOnline = isOnline;
            Students = students;
            IsFinished = isFinished;
            StudentsNotified = studentsNotified;
        }

        public new int MaxStudents
        {
            get => base.MaxStudents;
            set
            {
                ValidateMaxStudents(value);
                base.MaxStudents = value;
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

        public List<Weekday> Held
        {
            get => _held;
            set
            {
                ValidateHeld(value);
                _held = value;
            }
        }

        public new bool IsOnline { get; set; }
        public bool IsFinished { get; set; } = false;

        // If the best students were notified by the director
        public bool StudentsNotified { get; set; } = false;

        public int? CreatorId { get; set; } = null;

        public DateOnly StartDate
        {
            get => Date;
            set
            {
                ValidateDate(value);
                Date = value;
            }
        }

        public bool AreApplicationsClosed { get; set; }

        // TODO: Return different student IDs based on the status, only pending when accepting (ignore paused), remove all paused after starting a course etc.
        // TODO: Add logic to respective methods when a student drops out from, or others to resume their applications etc.
        // Dictionary of student IDs and their application status
        public Dictionary<int, ApplicationStatus> Students { get; } = new();

        // Dictionary of student IDs and their reasons for requesting to drop out
        public Dictionary<int, string> DropOutRequests { get; } = new();

        private static void ValidateDate(DateOnly startDate)
        {
            if ((startDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 7)
                throw new InvalidInputException("The course has to be at least 7 days from now.");
        }

        private void ValidateMaxStudents(int maxStudents)
        {
            if (!IsOnline && maxStudents <= 0)
                throw new InvalidInputException("You must pass the max number of students if the course is in-person.");
        }

        private static void ValidateHeld(List<Weekday> held)
        {
            if (held == null)
                throw new ArgumentNullException(nameof(held));
        }

        private static void ValidateDuration(int duration)
        {
            if (duration <= 0)
                throw new InvalidInputException("Invalid input: Duration must be greater than 0.");
        }

        public void AddStudent(int studentId)
        {
            if (Students.Count >= MaxStudents && !IsOnline)

                throw new InvalidInputException("The course is full.");

            if (!Students.TryAdd(studentId, ApplicationStatus.Pending))
                throw new InvalidInputException("Student has already applied to this course.");
        }

        public void RemoveStudent(int studentId)
        {
            if (!Students.ContainsKey(studentId))
                throw new InvalidInputException("Student hasn't applied to this course.");

            Students.Remove(studentId);
        }

        public void AddDropOutRequest(int studentId, string reason)
        {
            if (!Students.ContainsKey(studentId))
                throw new InvalidInputException("Student hasn't applied to this course.");
            reason =
                $"the student wants to withdraw from the course {Language} that started on {StartDate}. Reason : {reason}";
            if (!DropOutRequests.TryAdd(studentId, reason))
                throw new InvalidInputException("Student has already requested to drop out.");
        }
    }
}