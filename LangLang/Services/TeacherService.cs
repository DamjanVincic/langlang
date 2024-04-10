using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class TeacherService : ITeacherService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamService _examService = new ExamService();
    
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
}