﻿using System.Collections.Generic;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public interface ITeacherService
{
    public List<Teacher> GetAll();
    public List<Course> GetCourses(int teacherId, int pageIndex = 1, int? amount = null);
    public int GetCourseCount(int teacherId);
    public List<Exam> GetExams(int teacherId, int pageIndex = 1, int? amount = null);
    public List<Teacher> GetAvailableTeachers(Course course);
    public void RejectStudentApplication(int studentId, int courseId);
    public void ConfirmCourse(int courseId);
    public void FinishCourse(int courseId);
    public int GetExamCount(int teacherId);
}