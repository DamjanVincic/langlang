using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Services;

public interface IStudentService
{
    // TODO: Implement course and exam enrollment etc.
    public List<Student> GetAll();
    public List<Course> GetAvailableCourses(Student student);
    public List<Exam> GetAvailableExams();
    public void ApplyForCourse(Student student, int courseId);
}