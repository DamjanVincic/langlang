using System;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class ExamService : IExamService
{
    private readonly IExamFileRepository _examFileRepository = new ExamFileRepository();
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly IScheduleService _scheduleService = new ScheduleService();
    private readonly ILanguageService _languageService = new LanguageService();

    public List<Exam> GetAll()
    {
        return _examFileRepository.GetAll();
    }

    public Exam? GetById(int id)
    {
        return _examFileRepository.GetById(id);
    }

    public void Add(string languageName, LanguageLevel languageLevel, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");
        
        Language language = _languageService.GetLanguage(languageName, languageLevel) ??
                            throw new InvalidInputException("Language with the given level doesn't exist.");

        Exam exam = new Exam(language, maxStudents, examDate, teacherId, examTime)
        {
            Id = _examFileRepository.GenerateId()
        };

        _scheduleService.Add(exam);
        _examFileRepository.Add(exam);

        teacher.ExamIds.Add(exam.Id);
        _userRepository.Update(teacher);
    }

    public void Update(Language language, int maxStudents, DateOnly examDate, int teacherId, TimeOnly examTime)
    {
        // TODO: Check if teacher is allowed to update the exam
        // TODO: Decide which information should be updated
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        // TODO: Delete from schedule, students etc.
        
        Exam exam = _examFileRepository.GetById(id) ?? throw new InvalidInputException("Exam doesn't exist.");
        Teacher teacher = _userRepository.GetById(exam.TeacherId) as Teacher ??
                          throw new InvalidInputException("Teacher doesn't exist.");

        teacher.ExamIds.Remove(exam.Id);
        _userRepository.Update(teacher);
        
        _examFileRepository.Delete(id);
    }
}