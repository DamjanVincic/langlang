using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class StudentService : IStudentService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamRepository _examRepository = new ExamFileRepository();
    private readonly ILanguageRepository _languageRepository = new LanguageFileRepository();

    private readonly IUserService _userService = new UserService();
    private readonly IExamGradeService _examGradeService = new ExamGradeService();
    private readonly ICourseGradeService _courseGradeService = new CourseGradeService();
    private readonly IPenaltyPointService _penaltyPointService = new PenaltyPointService();


    public List<Student> GetAll()
    {
        return _userRepository.GetAll().OfType<Student>().ToList();
    }

    public List<Course> GetAvailableCourses(int studentId)
    {
        return _courseRepository.GetAll().Where(course =>

            (course.Students.Count < course.MaxStudents || course.IsOnline) &&

            (course.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 7 &&
            !course.Students.ContainsKey(studentId)).ToList();
    }

    public List<Course> GetAppliedCourses(int studentId)
    {
        Student? student = _userRepository.GetById(studentId) as Student;
        return student!.AppliedCourses.Select(courseId => _courseRepository.GetById(courseId)!).ToList();
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

        return _examRepository.GetAll().Where(exam =>
                exam.StudentIds.Count < exam.MaxStudents && IsNeededCourseFinished(exam, student) &&
                (exam.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days >= 30 && !IsAlreadyPassed(exam, student))
            .ToList();
    }

    /*
    if language from that course is in the dict than student has finished that course
    if its in the dict and it has value true then student passed exam, if its false he didnt pass it yet
    */
    // TODO: MNOC 3
    private bool IsNeededCourseFinished(Exam exam, Student student)
    {
        return student.LanguagePassFail.ContainsKey(exam.Language.Id) && student.LanguagePassFail[exam.Language.Id] == false;
    }

    private bool IsAlreadyPassed(Exam exam, Student student)
    {
        foreach (int languageId in student.LanguagePassFail.Keys)
        {
            Language language = _languageRepository.GetById(languageId)!;
            if (language.Name == exam.Language.Name && language.Level >= exam.Language.Level &&
                student.LanguagePassFail[languageId])
                return true;
        }

        return false;
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
        Exam appliedExam = _examRepository.GetById(examId)!;
        appliedExam.StudentIds.Add(student.Id);
        _examRepository.Update(appliedExam);
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

    // TODO: MNOC 3
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
    public void CheckIfFirstInMonth()
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        int dayOfMonth = currentDate.Day;

        if (dayOfMonth != 1 || UserService.LoggedInUser is not Student student || student.PenaltyPoints <= 0)
        {
            return;
        }

        --student.PenaltyPoints;
        _userRepository.Update(student);
        RemoveStudentPenaltyPoint(student.Id);
    }

    private void RemoveStudentPenaltyPoint(int studentId)
    {
        List<PenaltyPoint> penaltyPoints = _penaltyPointService.GetAll();

        foreach (PenaltyPoint point in penaltyPoints)
        {
            if (point.StudentId == studentId && !point.Deleted)
            {
                _penaltyPointService.Delete(point.Id);
                break;
            }
        }
    }

    // TODO: NOP 6
    public void AddPenaltyPoint(int studentId, PenaltyPointReason penaltyPointReason, int courseId,
        int teacherId, DateOnly datePenaltyPointGiven)
    {
        Student student = _userRepository.GetById(studentId) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");
        _ = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");
        ++student.PenaltyPoints;
        _userRepository.Update(student);
        _penaltyPointService.Add(penaltyPointReason, student.Id, courseId, teacherId, datePenaltyPointGiven);
        if (student.PenaltyPoints == 3)
        {
            _userService.Delete(student.Id);
        }
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

        ExamGrade examGrade = _examGradeService.GetById(examGradeId)!;
        student.LanguagePassFail[exam.Language.Id] = examGrade.Passed;

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

        // Update the logged in student
        if (UserService.LoggedInUser?.Id == studentId)
            _userService.Login(student.Email, student.Password);
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

    /// <summary>
    /// Pause all student applications except the one with the passed ID
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID which not to pause</param>
    public void PauseOtherApplications(int studentId, int courseId)
    {
        Student student = (_userRepository.GetById(studentId) as Student)!;
        
        foreach (Course course in student.AppliedCourses.Select(id => _courseRepository.GetById(id)!))
        {
            if (course.Id == courseId)
                continue;

            if (!course.Students.ContainsKey(studentId))
                throw new InvalidInputException($"Student hasn't applied to the course with ID {course.Id}.");
            
            course.Students[studentId] = ApplicationStatus.Paused;
            _courseRepository.Update(course);
        }
    }

    // TODO: Call this method when the teacher reviews the drop out request
    public void ResumeApplications(int studentId)
    {
        Student student = (_userRepository.GetById(studentId) as Student)!;

        foreach (Course course in student.AppliedCourses.Select(courseId => _courseRepository.GetById(courseId)!))
        {
            if (!course.Students.ContainsKey(studentId))
                throw new InvalidInputException($"Student hasn't applied to the course with ID {course.Id}.");
            
            course.Students[studentId] = ApplicationStatus.Pending;
            _courseRepository.Update(course);
        }
    }
}