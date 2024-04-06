using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace LangLang.Model
{
    public class Language
    {
        private string _name;

        private static readonly string LANGUAGE_FILE_NAME = "language.json";
        private static readonly string LANGUAGE_DIRECTORY_PATH = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataFiles");
        private static readonly string LANGUAGE_FILE_PATH = Path.Combine(Directory.GetCurrentDirectory(), "SourceDataFiles", LANGUAGE_FILE_NAME);

        private static List<Language> _languages = new List<Language>();
        private static List<string> _languageNames = new List<string>();

        public Language(string name, LanguageLevel level)
        {
            Name = name;
            Level = level;
            if (!Languages.Any(language => language.Equals(this)))
                Languages.Add(this);
            if (!LanguageNames.Any(languageName => languageName.Equals(name)))
            {
                LanguageNames.Add(name);
            }
        }

        public static void MakeLanguage(string languageName, LanguageLevel languageLevel)
        {
            if(LanguageNames.Contains(languageName))
            {
                foreach (Language language in Languages)
                {
                    if (language.Name.Equals(languageName) && language.Level == languageLevel)
                    {
                        return;
                    }
                }
            }
            else
            {
                LanguageNames.Add(languageName);
            }

            new Language(languageName, languageLevel);
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

        public static void LoadLanguagesFromJson()
        {
            try
            {
                string json = File.ReadAllText(LANGUAGE_FILE_PATH);
                _languages = JsonConvert.DeserializeObject<List<Language>>(json);
                foreach(Language language in _languages)
                {
                    _languageNames.Add(language.Name);
                }
            }
            catch (FileNotFoundException)
            {
                
            }
        }
        public static void WriteLanguageToJson()
        {
            if (!_languages.Any())
            {
                return;
            }
            
            string jsonExamString = JsonConvert.SerializeObject(_languages, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
            
            if (!Directory.Exists(LANGUAGE_DIRECTORY_PATH))
            {
                Directory.CreateDirectory(LANGUAGE_DIRECTORY_PATH);
            }
            
            File.WriteAllText(LANGUAGE_FILE_PATH, jsonExamString);
        }
    }
}
