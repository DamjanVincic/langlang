using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Language
    {
        private string _name;
        private static List<Language> _languages = new List<Language>();
        private static List<string> _languageNames = new List<string>();

        public Language(string name, LanguageLevel level)
        {
            Name = name;
            Level = level;
            Languages.Add(this);
            if (!LanguageNames.Contains(name))
            {
                LanguageNames.Add(name);
            }
        }

        public static List<Language> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
            }
        }
        public static List<string> LanguageNames
        {
            get => _languageNames;
            set
            {
                _languageNames = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                ValidateName(value);
                _name = value;
            }
        }
        public LanguageLevel Level { get; set; }

        private void ValidateName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (name.Equals(""))
            {
                throw new InvalidInputException("Name must include at least one character.");
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Language language &&
                   Name == language.Name &&
                   Level == language.Level;
        }

        public override string ToString()
        {
            return $"{Name} {Level}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name);
        }
    }
}
