using System;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class ExamService : IExamService
{
    private readonly IExamFileRepository _examFileRepository = new ExamFileRepository();
    private readonly IUserRepository _userRepository = new UserFileRepository();
    
    public List<Exam> GetAll()
    {
        return _examFileRepository.GetAll();
    }

    public Exam? GetById(int id)
    {
        return _examFileRepository.GetById(id);
    }
    
    public void Add(Language language, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ?? throw new InvalidInputException("User doesn't exist.");
        
        Exam exam = new Exam(language, maxStudents, examDate, teacherId, examTime);
        _examFileRepository.Add(exam);

        teacher.ExamIds.Add(exam.Id);
        _userRepository.Update(teacher);
    }
    
    public void Update(Language language, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime)
    {
        //TODO: Decide which information should be updated
        throw new NotImplementedException();
    }
    
    public void Delete(int id)
    {
        _examFileRepository.Delete(id);
    }
}