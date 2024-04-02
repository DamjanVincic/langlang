﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace LangLang.Model
{
    public class Language
    {
        private string _name;
        private const string LANGUAGE_FILE_PATH = @"C:\faks 2\usi\projekat\cp-usi-2024-3-b\LangLang\SourceDataFiles\language.json";
        private static List<Language> _languages = new List<Language>();
        private static List<string> _languageNames = new List<string>() { "Serbian","English","German"};

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
            catch (Exception ex)
            {
                Console.WriteLine("Error reading languages: " + ex.Message);
            }
        }
        public static void WriteLanguageToJson()
        {
            string jsonExamString = JsonConvert.SerializeObject(_languages);
            File.WriteAllText(LANGUAGE_FILE_PATH, jsonExamString);
        }
    }
}
