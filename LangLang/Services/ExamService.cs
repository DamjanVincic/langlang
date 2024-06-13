using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Windows.Input;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class ExamService : IExamService
{

    private readonly IExamRepository _examRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScheduleService _scheduleService;
    private readonly ILanguageService _languageService;
    private readonly IExamGradeRepository _examGradeRepository;
    private readonly IMessageService _messageService;
    
    public ExamService(IExamRepository examRepository, IUserRepository userRepository, IScheduleService scheduleService, ILanguageService languageService, IExamGradeRepository examGradeRepository, IMessageService messageService)
    {
        _examRepository = examRepository;
        _userRepository = userRepository;
        _scheduleService = scheduleService;
        _languageService = languageService;
        _examGradeRepository = examGradeRepository;
        _messageService = messageService;
    }

    public List<Exam> GetAll()
    {
        return _examRepository.GetAll();
    }

    public Exam? GetById(int id)
    {
        return _examRepository.GetById(id);
    }

    public Exam Add(string? languageName, LanguageLevel languageLevel, int maxStudents, DateOnly examDate, int? teacherId,
        TimeOnly examTime)
    {
        Teacher? teacher = null;
        if (teacherId != null)
            teacher = GetTeacherOrThrow(teacherId.Value);

        Language language = _languageService.GetLanguage(languageName, languageLevel) ??
                            throw new InvalidInputException("Language with the given level doesn't exist.");


        Exam exam = new(language, maxStudents, examDate, teacherId, examTime)
        { Id = _examRepository.GenerateId() };


        _scheduleService.Add(exam);
        _examRepository.Add(exam);

        if (teacher != null)
        {
            teacher.ExamIds.Add(exam.Id);
            _userRepository.Update(teacher);
        }
        return exam;
    }

    public void Update(int id, string languageName, LanguageLevel languageLevel, int maxStudents, DateOnly date,
        int? teacherId, TimeOnly time)
    {
        Exam exam = GetExamOrThrow(id);

        if ((exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 14)
            throw new InvalidInputException("The exam can't be changed if it's less than 2 weeks from now.");

        Teacher? teacher = null;
        if (teacherId.HasValue)
            teacher = GetTeacherOrThrow(teacherId.Value);


        Language language = _languageService.GetLanguage(languageName, languageLevel) ??
                            throw new InvalidInputException("Language with the given level doesn't exist.");

        int? oldTeacherId = exam.TeacherId;
        
        exam.Language = language;
        exam.MaxStudents = maxStudents;
        exam.Date = date;
        exam.ScheduledTime = time;
        exam.TeacherId = teacherId;

        // Validates if it can be added to the current schedule
        _scheduleService.Update(exam);

        Teacher? oldTeacher = null;
        if (oldTeacherId.HasValue)
            oldTeacher = GetTeacherOrThrow(oldTeacherId.Value);

        if (oldTeacher is not null)
        {
            oldTeacher.ExamIds.Remove(exam.Id);
            _userRepository.Update(oldTeacher);
        }

        if (teacher is not null)
        {
            teacher.ExamIds.Add(exam.Id);
            _userRepository.Update(teacher);
        }

        _examRepository.Update(exam);
    }

    public void Delete(int id)
    {
        Exam exam = GetExamOrThrow(id);
        Teacher? teacher = null;
        if (exam.TeacherId.HasValue)
            teacher = GetTeacherOrThrow(exam.TeacherId.Value);

        if (teacher is not null)
        {
            teacher.ExamIds.Remove(exam.Id);
            _userRepository.Update(teacher);
        }

        _scheduleService.Delete(id);
        
        foreach (Student? student in exam.StudentIds.Select(studentId => _userRepository.GetById(studentId) as Student))
        {
            student!.AppliedExams.Remove(exam.Id);
            _userRepository.Update(student);
        }

        _examRepository.Delete(id);
    }

    public void ConfirmExam(int examId)
    {
        Exam exam = GetExamOrThrow(examId);
        exam.Confirmed = true;
        _examRepository.Update(exam);
    }

    public List<Student> GetStudents(int examId)
    {
        Exam exam = _examRepository.GetById(examId)!;

        List<Student> students = exam.StudentIds.Select(studentId => (_userRepository.GetById(studentId) as Student)!).ToList();

        return students;
    }

    public List<Exam> GetStartableExams(int teacherId)
    {
        Teacher teacher = GetTeacherOrThrow(teacherId);
        List<Exam> startableExams = new();
        foreach (int examId in teacher.ExamIds)
        {
            Exam exam = GetExamOrThrow(examId);
            if ((exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days <= 7 &&
                !exam.Confirmed)
            {
                startableExams.Add(exam);
            }
        }

        return startableExams;
    }

    public int GetCurrentExam(int teacherId)
    {
        Teacher teacher = GetTeacherOrThrow(teacherId);
        foreach (int examId in teacher.ExamIds)
        {
            Exam exam = GetExamOrThrow(examId);

            double timeDifference = (DateTime.Now - exam.Date.ToDateTime(exam.ScheduledTime)).TotalMinutes;

            if (timeDifference >= 0 && timeDifference < Exam.ExamDuration)
                return exam.Id;
        }

        throw new InvalidInputException("There are currently no exams");
    }

    public void FinishExam(int examId)
    {
        Exam exam = GetExamOrThrow(examId);
        CheckGrades(exam);

        exam.TeacherGraded = true;
        _examRepository.Update(exam);
    }

    private void CheckGrades(Exam exam)
    {
        foreach (int studentId in exam.StudentIds)
        {
            Student student = _userRepository.GetById(studentId) as Student ??
                              throw new InvalidInputException("Student doesn't exist.");

            if (!student.ExamGradeIds.ContainsKey(exam.Id))
            {
                throw new InvalidInputException("Not all students have been graded.");
            }
        }
    }
    public List<Exam> GetUngradedExams()
    {
        return _examRepository.GetAll().Where(exam => exam.TeacherGraded == true && exam.DirectorGraded == false).ToList();
    }
    public void SendGrades(int examId)
    {
        Exam exam = _examRepository.GetById(examId)!;
        exam.DirectorGraded = true;
        _examRepository.Update(exam);

        foreach (User user in _userRepository.GetAll())
        {
            if (user is Student student && student.AppliedExams.Contains(examId))
            {
                student.AppliedExams.Remove(examId);
                _userRepository.Update(student);
            }
        }
        SendEmail(examId);
    }
    private void SendEmail(int examId)
    {
        Exam exam = _examRepository.GetById(examId)!;
        foreach (ExamGrade examGrade in _examGradeRepository.GetAll().Where(eg => eg.ExamId == examId))
        {
            string passedText = examGrade.Passed
                ? $"Congratulations, you have passed {exam.Language} exam!\n"
                : $"Unfortunately, you have failed {exam.Language} exam.\n";

            string pointsText = "Here are your points:\n" +
                                $"\tReading: {examGrade.ReadingPoints} \n" +
                                $"\tListening: {examGrade.ListeningPoints} \n" +
                                $"\tTalking: {examGrade.TalkingPoints} \n" +
                                $"\tWriting: {examGrade.WritingPoints} \n";

            _messageService.Add(examGrade.StudentId, passedText+pointsText);
            EmailService.SendMessage("Exam results",passedText+pointsText);
        }
    }
    private Teacher GetTeacherOrThrow(int teacherId)
    {
        Teacher? teacher = _userRepository.GetById(teacherId) as Teacher;
        if (teacher == null)
            throw new InvalidInputException("User doesn't exist.");
        return teacher;
    }
    private Exam GetExamOrThrow(int examId)
    {
        Exam? exam = _examRepository.GetById(examId);
        if (exam == null)
            throw new InvalidInputException("Exam doesn't exist.");
        return exam;
    }
}