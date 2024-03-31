using System;
using System.Collections.Generic;

namespace LangLang.Model
{
    public class Language
    {
        private string _name;
        private static List<Language> _laguages = new List<Language>();
        private static List<string> _laguageNames = new List<string>();

        public Language(string name, LanguageLevel level)
        {
            Name = name;
            Level = level;
            _laguages.Add(this);
            if (!_laguageNames.Contains(name))
            {
                _laguageNames.Add(name);
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
    }
}
