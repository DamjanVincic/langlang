using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Services;

public interface IStudentService
{
    // TODO: Implement course and exam enrollment etc.
    public List<Student> GetAll();
    public List<Course> GetAvailableCourses(int studentId);
    public List<Exam> GetAvailableExams();
    public void ApplyForCourse(int studentId, int courseId);
}