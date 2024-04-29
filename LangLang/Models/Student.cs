using System;
using System.Collections.Generic;

namespace LangLang.Models;

public class Student : User
{

    public Student(string firstName, string lastName, string email, string password, Gender gender, string phone, Education? education)
        : base(firstName, lastName, email, password, gender, phone)
    {
        Education = education ?? throw new ArgumentNullException(nameof(education));
    }
    public Education? Education { get; set; }
    public Course? ActiveCourse { get; set; }
    
    // obradjeniJezici / zavrseniJezici
    // dict jezik-bool, kada se zavrsi dodaj sa false, kada polozi ispit promeni na true
    public Dictionary<int, bool> CoursePassFail { get; set; } = new();
    public List<int> AppliedCourses { get; set; } = new();
}