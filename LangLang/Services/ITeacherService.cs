using System.Collections.Generic;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public interface ITeacherService
{
    public List<Teacher> GetAll();
    public List<Course> GetCourses(int teacherId);
    public List<Course> GetCoursesInRange(int teacherId, int startIndex, int amount);
    public int GetCourseCount(int teacherId);
    public List<Exam> GetExams(int teacherId);
    public List<Teacher> GetAvailableTeachers(Course course);
    public void RejectStudentApplication(int studentId, int courseId);
    public void ConfirmCourse(int courseId);
    public void FinishCourse(int courseId);
}