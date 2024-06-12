using LangLang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Services;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.FormTable
{
    public class Memory
    {
        private readonly ILanguageService _languageService;
        public Memory(ILanguageService languageService)
        {
            _languageService = languageService;
        }
        public static object GetValueFromInput(string input, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return input;
            }
            else if (targetType.IsPrimitive)
            {
                return Convert.ChangeType(input, targetType);
            }
            else if (targetType == typeof(List<Weekday>))
            {
                return ParseWeekdayList(input);
            }
            else if (Nullable.GetUnderlyingType(targetType) != null)
            {
                return ParseNullableType(input, targetType);
            }
            else if (targetType == typeof(DateOnly))
            {
                return DateOnly.Parse(input);
            }
            else if (targetType == typeof(TimeOnly))
            {
                return TimeOnly.Parse(input);
            }
            else if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                return ParseDictionary(input, targetType);
            }
            else if (targetType.IsPrimitive || targetType.IsEnum)
            {
                return ParseEnum(input,targetType);
            }
            // Add other type checks and conversions as needed
            else
            {
                throw new NotSupportedException($"Type {targetType.Name} is not supported");
            }
        }

        private static object ParseEnum(string input, Type targetType)
        {
            if (!targetType.IsEnum)
            {
                throw new ArgumentException("Target type must be an enum");
            }

            if (Enum.TryParse(targetType, input, out object enumValue))
            {
                return enumValue;
            }
            else if (Enum.IsDefined(targetType, input))
            {
                return Enum.Parse(targetType, input);
            }
            else
            {
                throw new ArgumentException($"Invalid input '{input}' for enum type '{targetType.Name}'");
            }
        }

        private static List<Weekday> ParseWeekdayList(string input)
        {
            var weekdays = input.Split(',').Select(day => Enum.Parse<Weekday>(day.Trim(), true)).ToList();
            return weekdays;
        }
        private static object ParseNullableType(string input, Type targetType)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "null")
            {
                return null;
            }
            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            return Convert.ChangeType(input, underlyingType);
        }
        private static object ParseDictionary(string input, Type targetType)
        {
            // Example format: key1:value1,key2:value2,key3:value3
            var keyValuePairs = input.Split(',')
                                      .Select(pair => pair.Split(':'))
                                      .ToDictionary(
                                            parts => Convert.ChangeType(parts[0].Trim(), targetType.GetGenericArguments()[0]),
                                            parts => Convert.ChangeType(parts[1].Trim(), targetType.GetGenericArguments()[1])
                                       );
            return keyValuePairs;
        }

        public void ShowLanguages()
        {
            List<Language> languages = _languageService.GetAll();

            Console.WriteLine("{0,-5} {1,-20} {2,-5} {3}", "ID", "Name", "Level", new string('-', 25));

            foreach (Language language in languages)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-5} {3}", language.Id, language.Name, language.Level, new string('-', 25));
            }
        }
    }
}
