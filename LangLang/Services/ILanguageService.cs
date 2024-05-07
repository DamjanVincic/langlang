﻿using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Services;

public interface ILanguageService
{
    public Dictionary<int, Language> GetAll();
    public List<string> GetAllNames();
    public Language? GetLanguage(string name, LanguageLevel level);
    public void Add(string name, LanguageLevel level);
}