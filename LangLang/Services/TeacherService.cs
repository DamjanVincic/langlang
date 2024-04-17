using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class TeacherService : ITeacherService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamService _examService = new ExamService();
    private readonly IUserService _userService = new UserService();
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

    public List<Course> GetInactiveCoursesCreatedByTeacher(int teacherId)
    {
        Teacher teacher = (Teacher)_userService.GetById(teacherId);
        List<Course> inactiveCoursesCreatedByTeacher = new List<Course>();

        foreach (int courseId in teacher.CourseIds)
        {
            Course course = _courseService.GetById(courseId);
            if (course.CreatorId == teacher.Id && !course.AreApplicationsClosed)
                inactiveCoursesCreatedByTeacher.Add(course);
        }

        return inactiveCoursesCreatedByTeacher;
    }

    public List<Course> GetActiveTeachersCourses(int teacherId)
    {
        Teacher teacher = (Teacher)_userService.GetById(teacherId);
        List<Course> activeTeachersCourses = new List<Course>();

        foreach (int courseId in teacher.CourseIds)
        {
            Course course = _courseService.GetById(courseId);
            if (course.AreApplicationsClosed)
                activeTeachersCourses.Add(course);
        }

        return activeTeachersCourses;
    }

    public List<Course> GetInactiveTeachersCoursesCreatedByDirector(int teacherId)
    {
        Teacher teacher = (Teacher)_userService.GetById(teacherId);
        List<Course> inactiveCoursesCreatedByDirector = new List<Course>();

        foreach (int courseId in teacher.CourseIds)
        {
            Course course = _courseService.GetById(courseId);
            if (course.CreatorId!=teacherId && !course.AreApplicationsClosed)
                inactiveCoursesCreatedByDirector.Add(course);
        }

        return inactiveCoursesCreatedByDirector;
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
}