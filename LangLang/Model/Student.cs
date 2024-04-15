using System;

namespace LangLang.Model;

public class Student : User
{
    public Education? Education { get; set; }

    public Student(string firstName, string lastName, string email, string password, Gender gender, string phone, Education? education)
        : base(firstName, lastName, email, password, gender, phone)
    {
        Education = education ?? throw new ArgumentNullException(nameof(education));
    }
}