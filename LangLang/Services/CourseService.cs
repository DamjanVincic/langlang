using System;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();

    public List<Course> GetAll()
    {
        return _courseRepository.GetAll();
    }

    public Course? GetById(int id)
    {
        return _courseRepository.GetById(id);
    }

    public void Add(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents, int creatorId,
        TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed, int teacherId, List<int> studentIds)
    {
        _courseRepository.Add(new Course(language, duration, held, isOnline, maxStudents, creatorId, scheduledTime,
            startDate, areApplicationsClosed, teacherId, studentIds));
    }

    public void Update(Language language, int duration, List<Weekday> held, bool isOnline, int maxStudents,
        int creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed, int teacherId,
        List<int> studentIds)
    {
        // TODO: Check if teacher is allowed to update the exam
        // TODO: Decide which information should be updated
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        _courseRepository.Delete(id);
    }
}