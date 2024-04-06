using System;
using System.Collections.Generic;
using System.Linq;

namespace LangLang.Model
{
    public class Teacher : User
    {
        private List<int> _courseIds = new();
        private List<int> _examIds  = new();
        
        public Teacher(string firstName, string lastName, string email, string password, Gender gender, string phone, List<Language> qualifications, int id = -1) : base(firstName, lastName, email, password, gender, phone, id)
        {
            validateQualifications(qualifications);
            Qualifications=qualifications;
            DateCreated=DateOnly.FromDateTime(DateTime.Now);
        }

        public List<Language> Qualifications { get; }
        public DateOnly DateCreated { get;}
        public List<int> CourseIds
        {
            get => _courseIds;
        }
        public List<int> ExamIds
        {
            get => _examIds;
        }

        private void validateQualifications(List<Language> qualificatons)
        {
            if (qualificatons.Except(Language.Languages).Any())
            {
                throw new InvalidInputException("Given language doesn't exist");
            }
        }

        private void validateCourseId(int courseId)
        {
            try
            {
                Course.GetById(courseId);
            }
            catch
            {
                throw new InvalidInputException("Given course doesn't exist");
            }
        }

        public void AddExam(int examId)
        {
            _examIds.Add(examId);
        }

        public void AddCourse(int courseId)
        {
            validateCourseId(courseId);
            _courseIds.Add(courseId);
        }
        
        public void DeleteExam(int examId)
        {
            _examIds.Remove(examId);
        }

        public void DeleteCourse(int courseId)
        {
            _courseIds.Remove(courseId);
        }
    }
}
