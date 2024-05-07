using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class StudentService : IStudentService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamRepository _examRepository = new ExamFileRepository();

    private readonly IUserService _userService = new UserService();
    private readonly IExamGradeService _examGradeService = new ExamGradeService();
    private readonly ICourseGradeService _courseGradeService = new CourseGradeService();

    public List<Student> GetAll()
    {
        return _userRepository.GetAll().OfType<Student>().ToList();
    }

    public List<Course> GetAvailableCourses(int studentId)
    {
        return _courseRepository.GetAll().Where(course =>
            course.Students.Count < course.MaxStudents &&
            (course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 7 &&
            !course.Students.ContainsKey(studentId)).ToList();
    }

    public List<Course> GetAppliedCourses(int studentId)
    {
        return _courseRepository.GetAll().Where(course => course.Students.ContainsKey(studentId)).ToList();
    }

    public List<Exam> GetAppliedExams(Student student)
    {
        var appliedExamIds = student.AppliedExams;

        var appliedExams = _examRepository.GetAll()
            .Where(exam => appliedExamIds.Contains(exam.Id))
            .ToList();

        return appliedExams;
    }


    /*
     1. student must have finished course for the language he wants to take exam in
     2. exam must have at least one available spot for student
     3. search date must be at least 30 days before the date the exam is held
     */
    public List<Exam> GetAvailableExams(Student student)
    {
        // Nakon što je učenik završio kurs, prikazuju mu se svi dostupni termini ispita koji se
        // odnose na jezik i nivo jezika koji je učenik obradio na kursu

        return _examRepository.GetAll().Where(exam => exam.StudentIds.Count < exam.MaxStudents && IsNeededCourseFinished(exam, student) && (exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 30).ToList();
    }

    /*
    if language from that course is in the dict than student has finished that course
    if its in the dict and it has value true then student passed exam, if its false he didnt pass it yet
    */
    private bool IsNeededCourseFinished(Exam exam, Student student)
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


    /*
     *  if teacher has graded the exam but director has not sent the email,
     *  then student can not apply for new exams
     */
    public void ApplyStudentExam(Student student, int examId)
    {
        if (student.AppliedExams.Contains(examId))
        {
            throw new InvalidInputException("You already applied");
        }
        if (_examRepository.GetById(examId) == null)
        {
            throw new InvalidInputException("Exam not found.");
        }
        foreach (int id in student.AppliedExams)
        {
            Exam exam = _examRepository.GetById(id)!;
            if (exam.TeacherGraded == true && exam.DirectorGraded == false)
            {
                throw new InvalidInputException("Cant apply for exam while waiting for results.");
            }
        }
        student.AppliedExams.Add(examId);
        _userRepository.Update(student);
    }

    public void DropExam(Exam exam, Student student)
    {
        if (_examRepository.GetById(exam.Id) == null)
        {
            throw new InvalidInputException("Exam not found.");
        }
        if (!student.AppliedExams.Contains(exam.Id))
        {
            throw new InvalidInputException("Exam does not exist");
        }
        if ((exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 10)
            throw new InvalidInputException("The exam can't be dropped if it's less than 10 days from now.");

        student.AppliedExams.Remove(exam.Id);
        _userRepository.Update(student);
    }

    public void ApplyForCourse(int studentId, int courseId)
    {
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        if (student.ActiveCourseId is not null)
            throw new InvalidInputException("You are already enrolled in a course.");
        
        Course course = _courseRepository.GetById(courseId) ??
                        throw new InvalidInputException("Course doesn't exist.");

        student.AddCourse(course.Id);
        course.AddStudent(student.Id);

        _userRepository.Update(student);
        _courseRepository.Update(course);
    }

    public void WithdrawFromCourse(int studentId, int courseId)
    {
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        Course course = _courseRepository.GetById(courseId) ??
                        throw new InvalidInputException("Course doesn't exist.");

        if ((course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 7)
            throw new InvalidInputException("The course can't be withdrawn from if it's less than 1 week from now.");

        student.RemoveCourse(course.Id);
        course.RemoveStudent(student.Id);

        _userRepository.Update(student);
        _courseRepository.Update(course);
    }

    public void DropActiveCourse(int studentId, string reason)
    {
        // TODO: Change the logic to send a reason to the teacher why the student wants to drop the course
        
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        if (student.ActiveCourseId is null)
            throw new InvalidInputException("You are not enrolled in a course.");
        
        Course course = _courseRepository.GetById(student.ActiveCourseId!.Value) ??
                        throw new InvalidInputException("Course doesn't exist.");

        if ((DateTime.Now - course.StartDate.ToDateTime(TimeOnly.MinValue)).Days < 7)
            throw new InvalidInputException("The course can't be dropped if it started less than a week ago.");

        course.AddDropOutRequest(studentId, reason);

        // TODO: Move this to when the teacher reviews the drop out request
        // student.DropActiveCourse();
        // course.RemoveStudent(student.Id);

        // _userRepository.Update(student);
        _courseRepository.Update(course);
    }

    public void ReportCheating(int studentId, int examId)
    {
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        Exam exam = _examRepository.GetById(examId) ?? throw new InvalidInputException("Exam doesn't exist.");

        exam.RemoveStudent(studentId);
        _examRepository.Update(exam);
        _userService.Delete(studentId);
    }
    
    public void Penalize(int studentId, int courseId)
    {
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        Course course = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");

        // TODO : inform the student about the penalty point and assign it to him
    }

    public void AddExamGrade(int studentId, int examId, int writing, int reading, int listening, int talking)
    {
        int examGradeId = _examGradeService.Add(examId, studentId, reading, writing, listening, talking);

        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

        Exam exam = _examRepository.GetById(examId) ?? throw new InvalidInputException("Exam doesn't exist.");

        if (student.ExamGradeIds.ContainsKey(examId))
            _examGradeService.Delete(student.ExamGradeIds[examId]);

        student.ExamGradeIds[examId] = examGradeId;

        //TODO : add passed to CoursePassFail, language doesn't have id?

        _userRepository.Update(student);
    }
    
    /// <summary>
    /// Reviews the teacher after the student has finished the course and removes the active course from the student
    /// </summary>
    public void ReviewTeacher(int studentId, int rating)
    {
        Student? student = _userRepository.GetById(studentId) as Student;

        Course? course = _courseRepository.GetById(student!.ActiveCourseId!.Value);

        Teacher? teacher = _userRepository.GetById(course!.TeacherId) as Teacher;
        
        teacher!.AddReview(rating);
        _userRepository.Update(teacher);
        
        student.DropActiveCourse();
        _userRepository.Update(student);
    }

    public void AddCourseGrade(int studentId, int courseId, int knowledgeGrade, int activityGrade)
    {
        int courseGradeId = _courseGradeService.Add(courseId, studentId, knowledgeGrade, activityGrade);

        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");
        _ = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");

        if (student.CourseGradeIds.ContainsKey(courseId))
            _courseGradeService.Delete(student.CourseGradeIds[courseId]);

        student.CourseGradeIds[courseId] = courseGradeId;

        _userRepository.Update(student);
    }
}