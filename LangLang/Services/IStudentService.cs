﻿using System;
using System.Collections.Generic;
using LangLang.Repositories;
using LangLang.Models;

namespace LangLang.Services;

public interface IStudentService
{
    public List<Student> GetAll();
    public List<Course> GetAvailableCourses(int studentId);
    public List<Course> GetAppliedCourses(int studentId);
    public List<Exam> GetAvailableExams(Student student);
    public List<Exam> GetAppliedExams(Student student);
    public void DropActiveCourse(int studentId);
    public void ApplyForCourse(int studentId, int courseId);
    public void WithdrawFromCourse(int studentId, int courseId);
    public void ApplyStudentExam(Student student, int examId);
    public void DropExam(Exam exam, Student student);
    public void ReportCheating(int studentId, int examId);

    public void AddExamGrade(int studentId, int examId, int writing, int reading, int listening, int talking);
}