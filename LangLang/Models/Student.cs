using System;
using System.Collections.Generic;
using LangLang.Views.CourseViews;

namespace LangLang.Models;

public class Student : User
{

    public Student(string firstName, string lastName, string email, string password, Gender gender, string phone, Education? education)
        : base(firstName, lastName, email, password, gender, phone)
    {
        Education = education ?? throw new ArgumentNullException(nameof(education));
        PenaltyPoints = 0;
        AppliedExams = new List<int>();
        ActiveCourseId = null;
    }
    public Education? Education { get; set; }
    public int PenaltyPoints { get; set; }
    public int? ActiveCourseId { get; set; }
    
    // obradjeniJezici / zavrseniJezici
    // dict jezik-bool, kada se zavrsi dodaj sa false, kada polozi ispit promeni na true
    public Dictionary<int, bool> CoursePassFail { get; set; } = new();
    public List<int> AppliedCourses { get; } = new();
    public List<int> AppliedExams { get; set; }

    public Dictionary<int, int> ExamGradeIds { get; set; } = new();
    
    public void AddCourse(int courseId)
    {
        if (AppliedCourses.Contains(courseId))
            throw new InvalidInputException("You have already applied to this course.");
        
        AppliedCourses.Add(courseId);
    }
    
    public void RemoveCourse(int courseId)
    {
        if (!AppliedCourses.Contains(courseId))
            throw new InvalidInputException("You haven't applied to this course.");
        
        AppliedCourses.Remove(courseId);
    }
}