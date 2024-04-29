using System;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public interface IStudentService
{
    // TODO: Implement course and exam enrollment etc.
    public List<Student> GetAll();
    public List<Course> GetAvailableCourses();
    public List<Exam> GetAvailableExams(Student student);
    public List<Exam> GetAppliedExams(Student student);

    public void ApplyStudentExam(Student student, int examId);
    public void DropExam(Exam exam, Student student);
}