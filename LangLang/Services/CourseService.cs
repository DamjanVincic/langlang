using System;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ILanguageService _languageService = new LanguageService();
    private readonly IScheduleService _scheduleService = new ScheduleService();

    public List<Course> GetAll()
    {
        return _courseRepository.GetAll();
    }

    public Course? GetById(int id)
    {
        return _courseRepository.GetById(id);
    }

    public void Add(string languageName, LanguageLevel languageLevel, int duration, List<Weekday> held, bool isOnline,
        int maxStudents, int creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed,
        int teacherId, List<int> studentIds)
    {
        Language language = _languageService.GetLanguage(languageName, languageLevel) ??
                            throw new InvalidInputException("Language with the given level doesn't exist.");

        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");

        Course course = new Course(_courseRepository.GenerateId(), language, duration, held, isOnline, maxStudents,
            creatorId, scheduledTime, startDate, areApplicationsClosed, teacherId, studentIds);

        // Validates if it can be added to the current schedule
        _scheduleService.Add(course);
        _courseRepository.Add(course);

        teacher.CourseIds.Add(course.Id);
        _userRepository.Update(teacher);
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
        Course course = _courseRepository.GetById(id) ?? throw new InvalidInputException("Course doesn't exist.");

        Teacher? teacher = _userRepository.GetById(course.TeacherId) as Teacher;

        teacher!.CourseIds.Remove(id);
        _userRepository.Update(teacher);
        
        _courseRepository.Delete(id);
    }
}