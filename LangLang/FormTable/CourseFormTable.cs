using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LangLang.Models;
using LangLang.Services;
namespace LangLang.FormTable;
public class CourseFormTable
{
    Type type = typeof(Course);
    private readonly ICourseService _courseService;
    private readonly ILanguageService _languageService;
    private List<Course> _courses;

    public CourseFormTable(ICourseService courseService, ILanguageService languageService)
    {
        _courseService = courseService;
        _courses = _courseService.GetAll();
        _languageService = languageService;
    }
    public void CourseMenu()
    {
        while (true)
        {
            Console.WriteLine("Chose option for course \n" +
                "1) create \n" +
                "2) update \n" +
                "3) delete \n" +
                "4) list all exams \n" +
                "q)uit");
            string userInput = Console.ReadLine();
            if (userInput == "1") 
                Add();
            if (userInput == "2")
                Update();
            if (userInput == "3")
                Delete();
            if(userInput == "4")
                Read();
            if (userInput == "q" || userInput == "Q")
                break;
            else
                Console.WriteLine("invalid option \n");
        }
    }
    public void Add()
    {
        PropertyInfo[] properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(Language))
            {
                ShowLanguages();
            }
            Console.Write($"Enter {property.Name} ({property.PropertyType.Name}): ");
            string input = Console.ReadLine();

            try
            {
                object value = GetValueFromInput(input, property.PropertyType);
                Console.WriteLine($"Successfully set {property.Name} to {value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting {property.Name}: {ex.Message}");
            }
        }
        Course newCourse = (Course)Activator.CreateInstance(type);

        _courses.Add(newCourse);
        Console.WriteLine("Course added successfully.\n");
    }
    private object GetValueFromInput(string input, Type targetType)
    {
        if (targetType == typeof(string))
        {
            return input;
        }
        else if (targetType.IsPrimitive || targetType.IsEnum)
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
        // Add other type checks and conversions as needed
        else
        {
            throw new NotSupportedException($"Type {targetType.Name} is not supported");
        }
    }

    private List<Weekday> ParseWeekdayList(string input)
    {
        var weekdays = input.Split(',').Select(day => Enum.Parse<Weekday>(day.Trim(), true)).ToList();
        return weekdays;
    }
    private object ParseNullableType(string input, Type targetType)
    {
        if (string.IsNullOrEmpty(input) || input.ToLower() == "null")
        {
            return null;
        }
        Type underlyingType = Nullable.GetUnderlyingType(targetType);
        return Convert.ChangeType(input, underlyingType);
    }
    private object ParseDictionary(string input, Type targetType)
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

    public void Update() { }
    public void Delete() { }
    public void Read() { }
    public void ShowLanguages()
    {
        List<Language> languages = _languageService.GetAll();

        Console.WriteLine("{0,-5} {1,-20} {2,-5} {3}", "ID", "Name", "Level");

        foreach (Language language in languages)
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-5} {3}", language.Id, language.Name, language.Level, new string('-', 25));
        }
    }

    public void Info()
    {
        Console.WriteLine("Type: " + type.Name);

        // Ispisivanje svojstava
        PropertyInfo[] properties = type.GetProperties();
        foreach (var property in properties)
        {
            Console.WriteLine("Property: " + property.Name);
        }

        // Ispisivanje metoda
        MethodInfo[] methods = type.GetMethods();
        foreach (var method in methods)
        {
            Console.WriteLine("Method: " + method.Name);
        }
        /*
        object instance = Activator.CreateInstance(type);

        // dobijanje metode
        MethodInfo method = type.GetMethod("MyMethod");

        // pozivanje metode dinamicki
        method.Invoke(instance, new object[] { "Hello, Reflection!" });
        */
    }
}