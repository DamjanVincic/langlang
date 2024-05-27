using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LangLang.Models;
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

    /// <summary>
    /// Get students with pending applications
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>List of students with pending applications</returns>
    public List<Student> GetStudents(int courseId)
    {
        Course course = GetById(courseId)!;
        List<Student> students = course.Students.Where(student => student.Value == ApplicationStatus.Pending)
            .Select(student => (_userRepository.GetById(student.Key) as Student)!).ToList();
        return students;
    }

    public List<Course> GetStartableCourses(int teacherId)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");
        List<Course> startableCourses = new();
        foreach (int courseId in teacher.CourseIds)
        {
            Course course = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");
            if ((course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days <= 7 &&
                !course.Confirmed)
            {
                startableCourses.Add(course);
            }
        }

        return startableCourses;
    }

    public List<Course> GetActiveCourses(int teacherId)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");
        List<Course> activeCourses = new();

        foreach (int courseId in teacher.CourseIds)
        {
            Course course = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");

            if ((DateTime.Now - course.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays >= 0 && course.Confirmed && !course.IsFinished)
            {
                activeCourses.Add(course);
            }
        }

        return activeCourses;
    }
    public List<Course> GetCoursesWithWithdrawals(int teacherId)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");
        List<Course> coursesWithWithdrawals = new();

        foreach (int courseId in teacher.CourseIds)
        {
            Course course = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");
            if (course.DropOutRequests.Any())
                coursesWithWithdrawals.Add(course);
        }
        return coursesWithWithdrawals;
    }


    // TODO: NOP 11
    public void Add(string languageName, LanguageLevel languageLevel, int duration, List<Weekday> held, bool isOnline,
        int maxStudents, int creatorId, TimeOnly scheduledTime, DateOnly startDate, bool areApplicationsClosed,
        int teacherId)
    {
        Language language = _languageService.GetLanguage(languageName, languageLevel) ??
                            throw new InvalidInputException("Language with the given level doesn't exist.");
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");

        startDate = SetValidStartDate(startDate, held);
        Course course = new(language, duration, held, isOnline, maxStudents, creatorId, scheduledTime, startDate,
            areApplicationsClosed, teacherId) { Id = _courseRepository.GenerateId() };

        _scheduleService.Add(course);
        _courseRepository.Add(course);
        teacher.CourseIds.Add(course.Id);
        _userRepository.Update(teacher);
    }

    // TODO: MELOC 24, NOP 9, MNOC 4
    public void Update(int id, int duration, List<Weekday> held,
        bool isOnline, int maxStudents, TimeOnly scheduledTime, DateOnly startDate,
        bool areApplicationsClosed, int teacherId)
    {
        Course course = _courseRepository.GetById(id) ?? throw new InvalidInputException("Course doesn't exist.");
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");

        if ((course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 7)
            throw new InvalidInputException("The course can't be changed if it's less than 1 week from now.");

        startDate = SetValidStartDate(startDate, held);
        course.Duration = duration;
        course.Held = held;
        course.IsOnline = isOnline;
        course.MaxStudents = maxStudents;
        course.StartDate = startDate;
        course.ScheduledTime = scheduledTime;
        course.AreApplicationsClosed = areApplicationsClosed;
        course.TeacherId = teacherId;

        _scheduleService.Update(course);

        if (teacher.Id != course.TeacherId)
        {
            Teacher? oldTeacher = _userRepository.GetById(course.TeacherId) as Teacher;
            oldTeacher!.CourseIds.Remove(course.Id);
            _userRepository.Update(oldTeacher);
            teacher.CourseIds.Add(course.Id);
            _userRepository.Update(teacher);
        }

        _courseRepository.Update(course);
    }

    public void Delete(int id)
    {
        Course course = _courseRepository.GetById(id) ?? throw new InvalidInputException("Course doesn't exist.");
        Teacher? teacher = _userRepository.GetById(course.TeacherId) as Teacher;

        teacher!.CourseIds.Remove(id);
        _userRepository.Update(teacher);

        foreach (Student student in course.Students.Keys.Select(studentId => (_userRepository.GetById(studentId) as Student)!))
        {
            student.RemoveCourse(course.Id);
            _userRepository.Update(student);
        }
        
        _scheduleService.Delete(id);
        _courseRepository.Delete(id);
    }

    private static DateOnly SetValidStartDate(DateOnly startDate, List<Weekday> held)
    {
        int a = (int)held[0] + 1;
        int b = (int)startDate.DayOfWeek;
        int difference = a - b;
        return startDate.AddDays((difference < 0 ? difference + 7 : difference) % 7);
    }
}