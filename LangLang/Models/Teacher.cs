using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Services;

namespace LangLang.Models
{
    public class Teacher : User
    {
        private List<Language> _qualifications = new();

        public Teacher(string firstName, string lastName, string email, string password, Gender gender, string phone,
            List<Language> qualifications) : base(firstName, lastName, email, password, gender, phone)
        {
            Qualifications = qualifications;
            DateCreated = DateOnly.FromDateTime(DateTime.Now);
        }

        public List<Language> Qualifications
        {
            get => _qualifications;
            set
            {
                ValidateQualifications(value);
                _qualifications = value;
            }
        }

        public DateOnly DateCreated { get; }
        public List<int> CourseIds { get; } = new();
        public List<int> ExamIds { get; } = new();

        private void ValidateQualifications(List<Language> qualifications)
        {
            var languageService = new LanguageService();
            if (qualifications.Any(q => !languageService.GetAll().ContainsValue(q)))
            {
                throw new InvalidInputException("Given language doesn't exist");
            }
        }
    }
}