using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Services;

public interface ITeacherService
{
    public List<Teacher> GetAll();
    public List<Course> GetCourses(int teacherId);
    public List<Exam> GetExams(int teacherId);
    public List<Teacher> GetAvailableTeachers(Course course);
    public void FinishCourse();
    public void AddLanguageToStudent(Student student, Course course);
    public void RejectStudentApplication(int studentId, int courseId);
    public void ConfirmCourse(int courseId);
    public void FinishCourse(int courseId);
}