using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository = new LanguageFileRepository();

    public List<Language> GetAll()
    {
        return _languageRepository.GetAll();
    }

    public List<string> GetAllNames()
    {
        return _languageRepository.GetAll().Select(language => language.Name).Distinct().ToList();
    }

    public Language? GetLanguage(string name, LanguageLevel level)
    {
        return _languageRepository.GetAll().FirstOrDefault(language => language.Name == name && language.Level == level);
    }

    public void Add(string name, LanguageLevel level)
    {
        _languageRepository.Add(new Language(name, level));
    }
}