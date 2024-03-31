using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Language
    {
        private string _name;
        public static List<Language> Languages = new List<Language>();
        public static List<string> LanguageNames = new List<string>();

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
    }
}
