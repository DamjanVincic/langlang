using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Language
    {
        private string _name;
        private LanguageLevel _level;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                ValidateName(value);
                _name = value;
            }
        }
        public LanguageLevel Level
        {
            get
            {
                return _level;
            }
            set
            {
                //ValidateLevel(value);
                _level = value;
            }
        }

        //public void ValidateLevel(LanguageLevel level)
        //{
        //    if (level == null)
        //    {
        //        throw new ArgumentNullException("Level must not be null.");
        //    }
        //}

        public void ValidateName(string name)
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
