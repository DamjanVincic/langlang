using System;
using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface ICourseService
{
    public List<Course> GetAll();
    public Course? GetById(int id);

    public void Add(string languageName, LanguageLevel languageLevel, int duration, List<Weekday> held, bool isOnline,
        int maxStudents, int creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed,
        int teacherId, List<int> studentIds);

    public void Update(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents,
        int creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed, int teacherId,
        List<int> studentIds);

    public void Delete(int id);
}