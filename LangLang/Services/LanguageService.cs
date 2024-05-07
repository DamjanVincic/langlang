using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository = new LanguageFileRepository();

    public Dictionary<int, Language> GetAll()
    {
        return _languageRepository.GetAll();
    }

    public List<string> GetAllNames()
    {
        return _languageRepository.GetAll().Select(language => language.Value.Name).Distinct().ToList();
    }

    public Language? GetLanguage(string name, LanguageLevel level)
    {
        return _languageRepository.GetAll()
            .FirstOrDefault(pair => pair.Value.Name == name && pair.Value.Level == level)
            .Value;
    }

    public void Add(string name, LanguageLevel level)
    {
        _languageRepository.Add(new Language(name, level));
    }
}