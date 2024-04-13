using System;
using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface IExamService
{
    public List<Exam> GetAll();
    public Exam? GetById(int id);
    public void Add(string languageName, LanguageLevel languageLevel, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime);
    public void Update(Language language, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime);
    public void Delete(int id);
}