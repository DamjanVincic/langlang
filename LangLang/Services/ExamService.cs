using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class ExamService : IExamService
{
    private readonly IExamRepository _examRepository = new ExamFileRepository();
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly IScheduleService _scheduleService = new ScheduleService();
    private readonly ILanguageService _languageService = new LanguageService();

    public List<Exam> GetAll()
    {
        return _examRepository.GetAll();
    }

    public Exam? GetById(int id)
    {
        return _examRepository.GetById(id);
    }

    public void Add(string languageName, LanguageLevel languageLevel, int maxStudents, DateOnly examDate, int teacherId,
        TimeOnly examTime)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");

        Language language = _languageService.GetLanguage(languageName, languageLevel) ??
                            throw new InvalidInputException("Language with the given level doesn't exist.");

        Exam exam = new Exam(language, maxStudents, examDate, teacherId, examTime)
            { Id = _examRepository.GenerateId() };

        _scheduleService.Add(exam);
        _examRepository.Add(exam);

        teacher.ExamIds.Add(exam.Id);
        _userRepository.Update(teacher);
    }

    public void Update(int id, string languageName, LanguageLevel languageLevel, int maxStudents, DateOnly date,
        int teacherId, TimeOnly time)
    {
        // TODO: Decide which information should be updated

        Exam exam = _examRepository.GetById(id) ?? throw new InvalidInputException("Exam doesn't exist.");

        if ((exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 14)
            throw new InvalidInputException("The exam can't be changed if it's less than 2 weeks from now.");

        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");

        Language language = _languageService.GetLanguage(languageName, languageLevel) ??
                            throw new InvalidInputException("Language with the given level doesn't exist.");

        exam.Language = language;
        exam.MaxStudents = maxStudents;
        exam.Date = date;
        exam.ScheduledTime = time;

        // Validates if it can be added to the current schedule
        _scheduleService.Update(exam);

        if (teacher.Id != exam.TeacherId)
        {
            Teacher? oldTeacher = _userRepository.GetById(exam.TeacherId) as Teacher;

            oldTeacher!.ExamIds.Remove(exam.Id);
            _userRepository.Update(oldTeacher);

            teacher.ExamIds.Add(exam.Id);
            _userRepository.Update(teacher);
        }

        _examRepository.Update(exam);
    }

    public void Delete(int id)
    {
        // TODO: Delete from schedule, students etc.

        Exam exam = _examRepository.GetById(id) ?? throw new InvalidInputException("Exam doesn't exist.");
        Teacher teacher = _userRepository.GetById(exam.TeacherId) as Teacher ??
                          throw new InvalidInputException("Teacher doesn't exist.");

        teacher.ExamIds.Remove(exam.Id);
        _userRepository.Update(teacher);

        _scheduleService.Delete(id);

        _examRepository.Delete(id);
    }

    public void ConfirmExam(int examId)
    {
        Exam exam = _examRepository.GetById(examId) ?? throw new InvalidInputException("Exam doesn't exist.");
        exam.Confirmed=true;
        _examRepository.Update(exam);
    }

    public List<Student> GetStudents(int examId)
    {
        Exam? exam = _examRepository.GetById(examId);

        List<Student> students = exam.StudentIds
            .Select(studentId => _userRepository.GetById(studentId) as Student)
            .Where(student => student != null)
            .Select(student => student!) 
            .ToList();

        return students;
    }

    public List<Exam> GetStartableExams(int teacherId)
    {
        Teacher teacher = _userRepository.GetById(teacherId) as Teacher ??
                          throw new InvalidInputException("User doesn't exist.");
        List<Exam> startableExams = new List<Exam>();
        foreach (int examId in teacher.ExamIds)
        {
            Exam exam = _examRepository.GetById(examId) ?? throw new InvalidInputException("Exam doesn't exist.");
            if((exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days <= 7 &&
                !exam.Confirmed)
            {
                startableExams.Add(exam);
            }
        }

        return startableExams;
    }

}