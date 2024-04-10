using System;

namespace LangLang.Model
{
    public class Language
    {
        private string _name = null!;

        public Language(string name, LanguageLevel level)
        {
            Name = name;
            Level = level;
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
            switch (name)
            {
                case null:
                    throw new ArgumentNullException(nameof(name));
                case "":
                    throw new InvalidInputException("Name must include at least one character.");
            }
        }

        public override string ToString()
        {
            return $"{Name} {Level}";
        }
    }
}
