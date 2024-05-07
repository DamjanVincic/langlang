using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Services;

public interface ITeacherService
{
    public List<Teacher> GetAll();
    public List<Course> GetCourses(int teacherId);
    public List<Exam> GetExams(int teacherId);
    public List<Teacher> GetAvailableTeachers(Course course);
    public void DeleteExams(int teacherId);
    public void RemoveFromInactiveCourses(int teacherId);
    public void DeleteInactiveCourses(int teacherId);
    public void FinishCourse();
    public void AddLanguageToStudent(Student student, Course course);
    public void RejectStudentApplication(int studentId, int courseId);
}