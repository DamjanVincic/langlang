﻿using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class StudentService : IStudentService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamRepository _examRepository = new ExamFileRepository();

    public List<Student> GetAll()
    {
        return _userRepository.GetAll().OfType<Student>().ToList();
    }

    public List<Course> GetAvailableCourses(int studentId)
    {
        // TODO: Validate to not show the courses that the student has already applied to and
        return _courseRepository.GetAll().Where(course =>
            course.StudentIds.Count < course.MaxStudents &&
            (course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 7 &&
            !course.StudentIds.Contains(studentId)).ToList();
    }

    public List<Course> GetAppliedCourses(int studentId)
    {
        return _courseRepository.GetAll().Where(course => course.StudentIds.Contains(studentId)).ToList();
    }

    public List<Exam> GetAvailableExams()
    {
        // TODO: Add checking if the student has finished the course and don't show the ones they have applied to
        return _examRepository.GetAll().Where(exam =>
            exam.StudentIds.Count < exam.MaxStudents &&
            (exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 30).ToList();
    }

    public void ApplyForCourse(int studentId, int courseId)
    {
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        Course course = _courseRepository.GetById(courseId) ??
                        throw new InvalidInputException("Course doesn't exist.");

        student.AddCourse(course.Id);
        course.AddStudent(student.Id);

        _userRepository.Update(student);
        _courseRepository.Update(course);
    }

    public void WithdrawFromCourse(int studentId, int courseId)
    {
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        Course course = _courseRepository.GetById(courseId) ??
                        throw new InvalidInputException("Course doesn't exist.");

        if ((course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 7)
            throw new InvalidInputException("The course can't be withdrawn from if it's less than 1 week from now.");

        student.RemoveCourse(course.Id);
        course.RemoveStudent(student.Id);

        _userRepository.Update(student);
        _courseRepository.Update(course);
    }
}