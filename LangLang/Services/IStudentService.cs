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
    public List<Exam> GetAvailableExams();
    public List<Exam> GetAppliedExams();

    public bool IsExamFull(Exam exam);
    public bool IsNeededCourseFinished(Exam exam);
    public bool IsAtLeast30DaysBeforeExam(Exam exam);

    public void ApplyStudentExam(Student student, int examId);
    public void DropExam(Exam exam);
}