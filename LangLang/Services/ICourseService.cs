using System;
using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface ICourseService
{
    public List<Course> GetAll();
    public Course? GetById(int id);

    public void Add(string languageName, LanguageLevel languageLevel, int duration, List<Weekday> held,
        bool isOnline, int maxStudents, int creatorId, TimeOnly scheduledTime, DateOnly startDate,
        bool areApplicationsClosed, int teacherId);

    public void Update(int id, string languageName, LanguageLevel languageLevel, int duration, List<Weekday> held,
        bool isOnline, int maxStudents, int creatorId, TimeOnly scheduledTime, DateOnly startDate,
        bool areApplicationsClosed, int teacherId);

    public void Delete(int id);
    public List<Course> GetTeachersCourses(int teacherId, bool active, bool createdByTeacher);
}