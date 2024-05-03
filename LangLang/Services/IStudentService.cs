using System;
using System.Collections.Generic;
using LangLang.Repositories;
using LangLang.Models;

namespace LangLang.Services;

public interface IStudentService
{
    // TODO: Implement course and exam enrollment etc.
    public List<Student> GetAll();
    public void Delete(int id);
    public List<Course> GetAvailableCourses(int studentId);
    public List<Course> GetAppliedCourses(int studentId);
    public List<Exam> GetAvailableExams(Student student);
    public List<Exam> GetAppliedExams(Student student);
    public void ApplyForCourse(int studentId, int courseId);
    public void WithdrawFromCourse(int studentId, int courseId);
    public void ApplyStudentExam(Student student, int examId);
    public void DropExam(Exam exam, Student student);
}