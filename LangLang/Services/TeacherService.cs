using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class TeacherService : ITeacherService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamService _examService = new ExamService();
    private readonly ICourseService _courseService = new CourseService();
    private readonly IScheduleService _scheduleService = new ScheduleService();

    public List<Teacher> GetAll()
    {
        return _userRepository.GetAll().OfType<Teacher>().ToList();
    }

    public List<Course> GetCourses(int teacherId)
    {
        return _courseRepository.GetAll().Where(course => course.TeacherId == teacherId).ToList();
    }

    public List<Exam> GetExams(int teacherId)
    {
        return _examService.GetAll().Where(exam => exam.TeacherId == teacherId).ToList();
    }

    public List<Teacher> GetAvailableTeachers(Course course)
    {
        List<Teacher> availableTeachers = new List<Teacher>();
        foreach (Teacher teacher_ in GetAll())
        {
            Course tempCourse = new Course(course.Language, course.Duration, course.Held, true,
                course.MaxStudents, course.CreatorId, course.ScheduledTime, course.StartDate,
                course.AreApplicationsClosed, teacher_.Id);

            if (_scheduleService.ValidateScheduleItem(tempCourse, true))
            {
                availableTeachers.Add(teacher_);
            }
        }

        return availableTeachers;
    }

    public void DeleteExams(int teacherId)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");

        foreach (int examId in teacher.ExamIds)
        {
            _examService.Delete(examId);
        }
    }

    public void RemoveFromInactiveCourses(int teacherId)
    {
        List<Course> courses = GetCourses(teacherId).Where(course => !course.AreApplicationsClosed 
                                                                     && course.CreatorId != teacherId).ToList();

        foreach (Course course in courses)
        {
            course.CreatorId = null;
            _courseRepository.Update(course);
        }
    }

    public void DeleteInactiveCourses(int teacherId)
    {
        List<Course> courses = GetCourses(teacherId).Where(course => !course.AreApplicationsClosed
                                                                     && course.CreatorId == teacherId).ToList();

        foreach (Course course in courses)
        {
            _courseService.Delete(course.Id);
        }
    }

    public void FinishCourse() { }

    /*
     *  student and course - selected Items
     */
    public void AddLanguageToStudent(Student student, Course course)
    {
        if(student.CoursePassFail.ContainsKey(course.Id))
        {
            return;
        }
        // finished but not passed
        student.CoursePassFail.Add(course.Id,false);
        _userRepository.Update(student);
    }
}