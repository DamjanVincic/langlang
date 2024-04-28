using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface ITeacherService
{
    public List<Teacher> GetAll();
    public List<Course> GetCourses(int teacherId);
    public List<Exam> GetExams(int teacherId);
    public List<Teacher> GetAvailableTeachers(Course course);
}