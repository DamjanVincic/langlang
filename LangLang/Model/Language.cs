using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Language
    {
        private string name;
        private LanguageLevel level;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                ValidateName(value);
                name = value;
            }
        }
        public LanguageLevel Level
        {
            get
            {
                return level;
            }
            set
            {
                ValidateLevel(value);
                level = value;
            }
        }

        private void ValidateLevel(LanguageLevel level)
        {
            if (level == null)
            {
                throw new ArgumentNullException("Level must not be null.");
            }
        }

        private void ValidateName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("Name must not be null.");
            }
            if (name.Equals(""))
            {
                throw new InvalidInputException("Name must include at least one character.");
            }
        }
        public Language(string name, LanguageLevel level) 
        {
            Name = name;
            Level = level;
        }
    }
}
