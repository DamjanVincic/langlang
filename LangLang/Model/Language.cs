using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Language
    {
        public string Name
        {
            get
            {
                return Name;
            }
            set
            {
                ValidateName(value);
                Name = value;
            }
        }
        public string Level
        {
            get
            {
                return Level;
            }
            set
            {
                ValidateLevel(value);
                Level = value;
            }
        }

        private void ValidateLevel(string level)
        {
            if (level == null)
            {
                throw new ArgumentNullException("Level must not be null.");
            }
            if (level.Equals(""))
            {
                throw new InvalidInputException("Level must include at least one character.");
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
        public Language(string name, string level) 
        {
            ValidateName(name);
            ValidateLevel(level);

            Name = name;
            Level = level;
        }
    }
}
