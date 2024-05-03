using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class UserService : IUserService
{
    public static User? LoggedInUser { get; private set; }

    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IPenaltyPointService _penaltyPointService = new PenaltyPointService();

    public List<User> GetAll()
    {
        return _userRepository.GetAll();
    }

    public User? GetById(int id)
    {
        return _userRepository.GetById(id);
    }

    public void Add(string firstName, string lastName, string email, string password, Gender gender, string phone,
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

    public void Update(int id, string firstName, string lastName, string password, Gender gender, string phone,
        Education? education = null, List<Language>? languages = null, int penaltyPoints = -1)
    {
        //TODO: Validate if user(student) hasn't applied to any courses or exams
        User user = _userRepository.GetById(id) ?? throw new InvalidInputException("User doesn't exist");

        if (user is Student studentCheck &&
            (studentCheck.AppliedCourses.Count > 0 || studentCheck.ActiveCourseId != null))
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

    public void CheckIfFirstInMonth()
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        int dayOfMonth = currentDate.Day;

        if (dayOfMonth != 1 || LoggedInUser is not Student student || student.PenaltyPoints <= 0)
        {
            return;
        }

        --student.PenaltyPoints;
        _userRepository.Update(student);
        RemoveStudentPenaltyPoint(student.Id);
    }

    private void RemoveStudentPenaltyPoint(int studentId)
    {
        List<PenaltyPoint> penaltyPoints = _penaltyPointService.GetAll();

        foreach (PenaltyPoint point in penaltyPoints)
        {
            if (point.StudentId == studentId && !point.Deleted)
            {
                _penaltyPointService.Delete(point.Id);
                break;
            }
        }
    }


    public void AddPenaltyPoint(Student student, PenaltyPointReason penaltyPointReason, bool deleted, int courseId,
        int teacherId, DateOnly datePenaltyPointGiven)
    {
        ++student.PenaltyPoints;
        _userRepository.Update(student);
        _penaltyPointService.Add(penaltyPointReason, deleted, student.Id, courseId, teacherId, datePenaltyPointGiven);
        if (student.PenaltyPoints == 3)
        {
            Delete(student.Id);
        }
    }
}