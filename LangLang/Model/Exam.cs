using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Linq;

namespace LangLang.Model
{
    public class Exam : ScheduleItem
    {
        public const int EXAM_DURATION = 360;

        private static readonly string baseDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private static readonly string EXAM_FILE_NAME = "exams.json";
        private static readonly string EXAM_FILE_PATH = Path.Combine(baseDirectory, "SourceDataFiles", EXAM_FILE_NAME);

        private static Dictionary<int, Exam> _exams = new Dictionary<int, Exam>();
        private Language _language;
        private int _maxStudents;
        private DateOnly _examDate;


        public Exam(Language language, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime) : base(teacherId, examTime)
        {
            Language = language;
            MaxStudents = maxStudents;
            ExamDate = examDate;
            StudentIds = new List<int>();
            _exams.Add(Id, this);
        }

        public static List<Exam> GetTeacherExams(int teacherId)
        {
            return _exams.Values.Where(exam => exam.TeacherId == teacherId).ToList();
        }

        public List<int> StudentIds { get; set; }

        public Language Language
        {
            get => _language;
            set
            {
                ValidateLanguage(value);
                _language = value;
            }
        }

        public int MaxStudents
        {
            get => _maxStudents;
            set
            {
                ValidateMaxStudents(value);
                _maxStudents = value;
            }
        }

        public DateOnly ExamDate
        {
            get => _examDate;
            set
            {
                ValidateExamDate(value);
                _examDate = value;
            }
        }

        public void ValidateMaxStudents(int maxStudents)
        {
            if (maxStudents < 0)
            {
                throw new InvalidInputException("Number of max students can not be negative.");
            }
        }

        public void ValidateLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }
        }

        public void ValidateExamDate(DateOnly examDate)
        {
            DateOnly today = new DateOnly();
            if (examDate < today)
            {
                throw new ArgumentNullException("Date of exam must be after today.");
            }
        }
        public static Exam GetById(int id)
        {
            if (_exams.ContainsKey(id))
            {
                return _exams[id];
            }
            else
            {
                return null;
            }
        }
        
        public void Delete()
        {
            _exams.Remove(Id);
        }
        
        public static void LoadExamFromJson()
        {
            try
            {
                using (StreamReader r = new StreamReader(EXAM_FILE_PATH))
                {
                    string json = r.ReadToEnd();
                    Dictionary<int, Exam> exams = JsonConvert.DeserializeObject<Dictionary<int, Exam>>(json);

                    foreach (var kvp in exams)
                    {
                        _exams.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading emaxs from JSON: " + ex.Message);
            }
        }

        public static void WriteExamToJson()
        {
            string jsonExamString = JsonConvert.SerializeObject(_exams);
            File.WriteAllText(EXAM_FILE_PATH, jsonExamString);
            
        }

        public static List<Exam> GetAvailableExams()
        {
            //TODO: Add checking if the student has finished the course and don't show the ones they have applied to
            return _exams.Values.Where(exam => exam.StudentIds.Count < exam.MaxStudents && (exam.ExamDate.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber) >= 30).ToList();
        }
    }
}
