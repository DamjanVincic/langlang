using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class StudentService : IStudentService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly IExamFileRepository _examFileRepository = new ExamFileRepository();
    
    public List<Student> GetAll()
    {
        return _userRepository.GetAll().OfType<Student>().ToList();
    }
    
    public List<Exam> GetAvailableExams()
    {
        //TODO: Add checking if the student has finished the course and don't show the ones they have applied to
        return _examFileRepository.GetAll().Where(exam => exam.StudentIds.Count < exam.MaxStudents && (exam.ExamDate.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber) >= 30).ToList();
    }
}