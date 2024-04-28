using System;
using System.Collections.Generic;

namespace LangLang.Model;

public class Student : User
{

    public Student(string firstName, string lastName, string email, string password, Gender gender, string phone, Education? education)
        : base(firstName, lastName, email, password, gender, phone)
    {
        Education = education ?? throw new ArgumentNullException(nameof(education));
        PenaltyPoints = 0;
        ActiveCourse = null;
        LanguagesPassFail = new Dictionary<Language, bool>();
        AppliedCourses = new List<int>();
        AppliedExams = new List<int>(); 
    }
    public Education? Education { get; set; }
    public int PenaltyPoints { get; set; }
    public Course ActiveCourse { get; set; }
    // obradjeniJezici / zavrseniJezici
    // dict jezik-bool, kada se zavrsi dodaj sa false, kada polozi ispit promeni na true
    public Dictionary<Language,bool> LanguagesPassFail { get; set; }
    public List<int> AppliedCourses {  get; set; }
    public List<int> AppliedExams { get; set; }
}