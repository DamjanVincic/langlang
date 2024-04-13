using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();

    public List<User> GetAll()
    {
        return _userRepository.GetAll();
    }

    public User? GetById(int id)
    {
        return _userRepository.GetById(id);
    }
    
    public User? GetByEmail(string email)
    {
        return _userRepository.GetAll().FirstOrDefault(user => user.Email.Equals(email));
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
    }

    public void Delete(int id)
    {
        _userRepository.Delete(id);
    }

    public User? Login(string email, string password)
    {
        return _userRepository.GetAll().FirstOrDefault(user => user.Email.Equals(email) && user.Password.Equals(password));
    }
}