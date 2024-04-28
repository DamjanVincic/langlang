using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface IStudentService
{
    // TODO: Implement course and exam enrollment etc.
    public List<Student> GetAll();
    public List<Course> GetAvailableCourses();
    public List<Exam> GetAvailableExams();
    public void ApplyStudentExam(Student student, int examId);

}