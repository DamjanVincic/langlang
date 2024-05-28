﻿using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class UserService : IUserService
{
    public static User? LoggedInUser { get; private set; }

    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IExamRepository _examRepository;
    private readonly ICourseService _courseService;
    private readonly IExamService _examService;

    public UserService(IUserRepository userRepository, ICourseRepository courseRepository, IExamRepository examRepository,
        ICourseService courseService, IExamService examService)
    {
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _examRepository = examRepository;
        _courseService = courseService;
        _examService = examService;
    }

    public List<User> GetAll()
    {
        return _userRepository.GetAll();
    }

    public User? GetById(int id)
    {
        return _userRepository.GetById(id);
    }

    // TODO: NOP 8
    public void Add(string? firstName, string? lastName, string? email, string? password, Gender gender, string? phone,
        Education? education = null, List<Language>? languages = null)
    {
        if (_userRepository.GetAll().Any(user => user.Email.Equals(email)))
            throw new InvalidInputException("Email already exists");

        if (education is not null)
            _userRepository.Add(new Student(firstName, lastName, email, password, gender, phone, education));
        else if (languages is not null)
            _userRepository.Add(new Teacher(firstName, lastName, email, password, gender, phone, languages));
        else
            throw new InvalidInputException("Invalid input");
    }

    // TODO: MELOC 22, CYCLO_SWITCH 8, NOP 9, MNOC 5
    public void Update(int id, string firstName, string lastName, string password, Gender gender, string phone,
        Education? education = null, List<Language>? languages = null, int penaltyPoints = -1)
    {
        User user = _userRepository.GetById(id) ?? throw new InvalidInputException("User doesn't exist");

        if (user is Student studentCheck && (studentCheck.AppliedCourses.Any() || studentCheck.ActiveCourseId != null ||
                                             studentCheck.AppliedExams.Any()))
            throw new InvalidInputException(
                "You cannot change your information if you have applied to, or enrolled in any courses");

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Password = password;
        user.Gender = gender;
        user.Phone = phone;

        switch (user)
        {
            case Student student:
                student.PenaltyPoints = penaltyPoints != -1 ? penaltyPoints : student.PenaltyPoints;
                student.Education = education;
                break;
            case Teacher teacher:
                // TODO: Uncomment if teacher gets allowed to update their qualifications
                // teacher.Qualifications = languages ?? new List<Language>();
                break;
        }

        _userRepository.Update(user);

        if (LoggedInUser?.Id == id)
            LoggedInUser = user;
    }

    public void Delete(int id)
    {
        User user = _userRepository.GetById(id) ?? throw new InvalidOperationException("User doesn't exist");

        switch (user)
        {
            case Student student:
                DeleteStudent(student);
                break;
            case Teacher teacher:
                DeleteTeacher(teacher);
                break;
        }

        _userRepository.Delete(id);

        if (LoggedInUser?.Id == id)
            LoggedInUser = null;
    }

    public User? Login(string email, string password)
    {
        User? user = _userRepository.GetAll()
            .FirstOrDefault(user => !user.Deleted && user.Email.Equals(email) && user.Password.Equals(password));
        LoggedInUser = user;
        return user;
    }

    public void Logout()
    {
        if (LoggedInUser == null)
            throw new InvalidInputException("Already logged out.");

        LoggedInUser = null;
    }

    private void DeleteStudent(Student student)
    {
        foreach (var course in student.AppliedCourses.Select(courseId =>
                     _courseRepository.GetById(courseId) ??
                     throw new InvalidOperationException("Course doesn't exist")))
        {
            course.RemoveStudent(student.Id);
            _courseRepository.Update(course);
        }

        foreach (Exam exam in _examService.GetAll())
        {
            // remove student from exams only ih exam was not held
            if (exam.StudentIds.Contains(student.Id) && exam.TeacherGraded != true)
            {
                exam.StudentIds.Remove(student.Id);
                _examRepository.Update(exam);
            }
        }

        if (student.ActiveCourseId is null) return;

        Course enrolledCourse = _courseRepository.GetById(student.ActiveCourseId!.Value) ??
                                throw new InvalidOperationException("Course doesn't exist");
        enrolledCourse.RemoveStudent(student.Id);
        _courseRepository.Update(enrolledCourse);
    }

    private void DeleteTeacher(Teacher teacher)
    {
        // Delete exams
        foreach (int examId in teacher.ExamIds)
        {
            _examService.Delete(examId);
        }

        List<Course> courses = _courseRepository.GetAll()
            .Where(course => course.TeacherId == teacher.Id && !course.AreApplicationsClosed).ToList();

        foreach (Course course in courses)
        {
            // Delete inactive courses
            if (course.CreatorId == teacher.Id)
                _courseService.Delete(course.Id);
            else
            {
                // Remove from inactive courses
                course.TeacherId = null;
                _courseRepository.Update(course);
            }
        }
    }
}