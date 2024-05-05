using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class UserService : IUserService
{
    public static User? LoggedInUser { get; private set; }
    
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ITeacherService _teacherService = new TeacherService();

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
        Education? education = null, List<Language>? languages = null)
    {
        //TODO: Validate if user(student) hasn't applied to any courses or exams
        User user = _userRepository.GetById(id) ?? throw new InvalidInputException("User doesn't exist");

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Password = password;
        user.Gender = gender;
        user.Phone = phone;

        switch (user)
        {
            case Student student:
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

        User user = _userRepository.GetById(id) ?? throw new InvalidInputException("User doesn't exist");

        switch (user)
        {
            case Teacher teacher:
                DeleteTeacher(id);
                break;
        }

        if (LoggedInUser?.Id == id)
            LoggedInUser = null;
    }

    public User? Login(string email, string password)
    {
        User? user = _userRepository.GetAll().FirstOrDefault(user => user.Email.Equals(email) && user.Password.Equals(password));
        LoggedInUser = user;
        return user;
    }
    
    public void Logout()
    {
        if (LoggedInUser == null)
            throw new InvalidInputException("Already logged out.");
        
        LoggedInUser = null;
    }

    private void DeleteTeacher(int teacherId)
    {
        _teacherService.DeleteExams(teacherId);
        _teacherService.RemoveFromInactiveCourses(teacherId);
        _teacherService.DeleteInactiveCourses(teacherId);
    }
}