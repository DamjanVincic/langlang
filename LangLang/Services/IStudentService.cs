﻿using System;
using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Services;

public interface IStudentService
{
    // TODO: Implement course and exam enrollment etc.
    public List<Student> GetAll();
    public List<Course> GetAvailableCourses(int studentId, int pageIndex = 1, int? amount = null);
    public List<Course> GetAppliedCourses(int studentId, int pageIndex = 1, int? amount = null);
    public List<Exam> GetAvailableExams(Student student);
    public List<Exam> GetAppliedExams(Student student);
    public void ApplyForCourse(int studentId, int courseId);
    public void WithdrawFromCourse(int studentId, int courseId);
    public void ApplyStudentExam(Student student, int examId);
    public void DropExam(Exam exam, Student student);
    public void ReportCheating(int studentId, int examId);

    public void AddExamGrade(int studentId, int examId, int writing, int reading, int listening, int talking);
    public void AddCourseGrade(int studentId, int courseId, int knowledgeGrade, int activityGrade);
    public void CheckIfFirstInMonth();
    public void AddPenaltyPoint(int studentId, PenaltyPointReason penaltyPointReason, int courseId,
                                int teacherId, DateOnly datePenaltyPointGiven);
    public void ReviewTeacher(int id, int response);
    public void DropActiveCourse(int id, string responseText);
    public void PauseOtherApplications(int studentId, int courseId);
    public void ResumeApplications(int studentId);
}