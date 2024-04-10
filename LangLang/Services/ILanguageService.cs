using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface ILanguageService
{
    public List<Language> GetAll();
    public List<string> GetAllNames();
    public void Add(string name, LanguageLevel level);
}