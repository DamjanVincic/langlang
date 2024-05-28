﻿using System.Collections.Generic;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public interface ITeacherService
{
    public List<Teacher> GetAll();

    public List<Teacher> GetPage(int pageIndex = 1, int? amount = null, string propertyName = "", string sortingWay = "ascending");
    public int Count();
    public List<Course> GetCourses(int teacherId, int pageIndex = 1, int? amount = null, string propertyName = "", string sortingWay = "ascending");
    public int GetCourseCount(int teacherId);
    public List<Exam> GetExams(int teacherId);
    public List<Teacher> GetAvailableTeachers(Course course);
    public void RejectStudentApplication(int studentId, int courseId);
    public void ConfirmCourse(int courseId);
    public void FinishCourse(int courseId);
}