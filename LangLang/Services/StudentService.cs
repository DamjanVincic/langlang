using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class StudentService : IStudentService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamRepository _examRepository = new ExamFileRepository();
    private Student student = UserService.LoggedInUser as Student ??
                                  throw new InvalidOperationException("No one is logged in.");

    public List<Student> GetAll()
    {
        return _userRepository.GetAll().OfType<Student>().ToList();
    }


    public List<Course> GetAvailableCourses()
    {
        // TODO: Validate to not show the courses that the student has already applied to and
        return _courseRepository.GetAll().Where(course =>
            course.StudentIds.Count < course.MaxStudents &&
            (course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 7).ToList();
    }

    public List<Exam> GetAppliedExams()
    {
        List<Exam> appliedExams = new List<Exam>();
        var allExams = _examRepository.GetAll();
        var appliedExamIds = student.AppliedExams;

        for (int i = 0; i < allExams.Count; i++)
        {
            if (appliedExamIds.Contains(allExams[i].Id))
            {
                appliedExams.Add(allExams[i]);
            }
        }

        return appliedExams;
    }

    /*
     1. student must have finished course for the language he wants to take exam in
     2. exam must have at least one available spot for student
     3. search date must be at least 30 days before the date the exam is held
     */
    public List<Exam> GetAvailableExams()
    {
        // Nakon što je učenik završio kurs, prikazuju mu se svi dostupni termini ispita koji se
        // odnose na jezik i nivo jezika koji je učenik obradio na kursu
        
       return _examRepository.GetAll().Where(exam => !IsExamFull(exam) && IsNeededCourseFinished(exam) && IsAtLeast30DaysBeforeExam(exam)).ToList();
    }

    public bool IsExamFull(Exam exam)
    {
        bool value =  exam.StudentIds.Count >= exam.MaxStudents;
        return value;
    }

    /*
    if language from that course is in the dict than student has finished that course
    if its in the dict and it has value true then student passed exam, if its false he didnt pass it yet
    */
    public bool IsNeededCourseFinished(Exam exam)
    {
        return student.CoursePassFail.Any(coursePassFaild =>
        {
            Course? course = _courseRepository.GetById(coursePassFaild.Key);
            return course != null &&
                   course.Language.Name == exam.Language.Name &&
                   course.Language.Level == exam.Language.Level &&
                   !coursePassFaild.Value;
        });
    }
    public bool IsAtLeast30DaysBeforeExam(Exam exam)
    {
        return (exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 30;
    }

    public void ApplyStudentExam(Student student, int examId)
    {
        if (student.AppliedExams.Contains(examId))
        {
            throw new Exception("You already applied");
        }

        student.AppliedExams.Add(examId);
        _userRepository.Update(student);
    }

    public void DropExam(int examId)
    {
        if (!student.AppliedExams.Contains(examId))
        {
            throw new Exception("Exam does not exist");
        }

        student.AppliedExams.Remove(examId);
        _userRepository.Update(student);
    }
}