using System.Collections.Generic;
using System.IO;
using System.Linq;
using LangLang.Models;
using Newtonsoft.Json;

namespace LangLang.Repositories;

public class LanguageFileRepository : ILanguageRepository
{
    private const string LanguageFileName = "languages.json";
    private const string LanguageDirectoryName = "data";

    private List<Language> _languages = new();

    public List<Language> GetAll()
    {
        LoadData();
        return _languages;
    }

    public void Add(Language language)
    {
        LoadData();

        if (_languages.Any(lang => lang.Name == language.Name && lang.Level == language.Level))
            throw new InvalidInputException("Language already exists.");

        _languages.Add(language);

        SaveData();
    }

    private void SaveData()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), LanguageDirectoryName, LanguageFileName);

        string json = JsonConvert.SerializeObject(_languages, new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        });

        File.WriteAllText(filePath, json);
    }

    private void LoadData()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), LanguageDirectoryName, LanguageFileName);

        if (!File.Exists(filePath)) return;

        string json = File.ReadAllText(filePath);
        _languages = JsonConvert.DeserializeObject<List<Language>>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        }) ?? new List<Language>();
    }
}