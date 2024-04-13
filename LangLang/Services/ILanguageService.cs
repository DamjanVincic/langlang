using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface ILanguageService
{
    public List<Language> GetAll();
    public List<string> GetAllNames();
    public Language? GetLanguage(string name, LanguageLevel level);
    public void Add(string name, LanguageLevel level);
}