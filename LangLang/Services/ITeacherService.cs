using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface ITeacherService
{
    public List<Teacher> GetAll();
    public List<Course> GetCourses(int teacherId);
    public List<Exam> GetExams(int teacherId);
    public List<Course> GetInactiveCoursesCreatedByTeacher(int teacherId);
    public List<Course> GetActiveTeachersCourses(int teacherId);
    public List<Course> GetInactiveTeachersCoursesCreatedByDirector(int teacherId);
}