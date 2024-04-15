using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class StudentService : IStudentService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamRepository _examRepository = new ExamFileRepository();

    public List<Student> GetAll()
    {
        return _userRepository.GetAll().OfType<Student>().ToList();
    }

    public List<Course> GetAvailableCourses()
    {
        // TODO: Validate to not show the courses that the student has already applied to and
        return _courseRepository.GetAll().Where(course =>
            course.StudentIds.Count < course.MaxStudents &&
            (course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 7).ToList();
    }

    public List<Exam> GetAvailableExams()
    {
        // TODO: Add checking if the student has finished the course and don't show the ones they have applied to
        return _examRepository.GetAll().Where(exam =>
            exam.StudentIds.Count < exam.MaxStudents &&
            (exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 30).ToList();
    }
}