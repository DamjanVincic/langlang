using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LangLang.Model
{
    public class Course : ScheduleItem
    { 
        public const int CLASS_DURATION = 90;
        private static Dictionary<int, Course> _courses = new Dictionary<int, Course>();

        private static readonly string COURSE_FILE_NAME = "courses.json";
        private static readonly string COURSE_DIRECTORY_PATH = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataFiles");
        private static readonly string COURSE_FILE_PATH = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataFiles", COURSE_FILE_NAME);
        
        private Language _language;
        private int _duration;
        private List<Weekday> _held;
        private int _maxStudents;
        private DateOnly _startDate;
        private static List<int> _courseIds = new List<int>();

        public Course(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents, int creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed, int teacherId, List<int> studentIds, int id = -1) : base(teacherId, scheduledTime, id)
        {
            Language = language;
            Duration = duration;
            Held = held;
            CreatorId = creatorId;
            AreApplicationsClosed = areApplicationsClosed;
            IsOnline = isOnline;
            MaxStudents = maxStudents;
            StartDate = startDate;
            StudentIds = studentIds;
            _courses.Add(Id, this);
            CourseIds.Add(Id);
            Schedule.ModifySchedule(this, StartDate, Duration, null, Held);
            
        }

        public static List<Course> GetTeacherCourses(int teacherId)
        {
            return _courses.Values.Where(course => course.TeacherId == teacherId).ToList();
        }
        
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
        public List<int> StudentIds { get; set; }
        public static Dictionary<int, Course> Courses { get => _courses; set => _courses = value; }

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
            if (!IsOnline && maxStudents <= 0)
            {
                throw new InvalidInputException("You must pass the max number of students if the course is in-person.");
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
            return Courses[id];
        }

        public static void LoadCourseFromJson()
        {
            try
            {
                using (StreamReader r = new StreamReader(COURSE_FILE_PATH))
                {
                    string json = r.ReadToEnd();
                    Dictionary<int, Course> exams = JsonConvert.DeserializeObject<Dictionary<int, Course>>(json);

                    // foreach (var kvp in exams)
                    // {
                    //     _courses.Add(kvp.Key, kvp.Value);
                    // }
                    
                    IdCounter = Math.Max(IdCounter, _courses.Keys.Max() + 1);
                }
            }
            catch (FileNotFoundException)
            {
                
            }
        }

        public static void WriteCourseToJson()
        {
            if (!_courses.Any())
            {
                return;
            }
            
            string jsonExamString = JsonConvert.SerializeObject(_courses, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

            if (!Directory.Exists(COURSE_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(COURSE_DIRECTORY_PATH);
            }
            
            File.WriteAllText(COURSE_FILE_PATH, jsonExamString);
        }

        public static List<Course> GetAvailableCourses()
        {
            //TODO: Validate to not show the courses that the student has already applied to and
            return _courses.Values.Where(course => course.StudentIds.Count < course.MaxStudents && (course.StartDate.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber) >= 7).ToList();
        }
    }
}
