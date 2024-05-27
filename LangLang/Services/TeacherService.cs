using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class TeacherService : ITeacherService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly ICourseRepository _courseRepository = new CourseFileRepository();
    private readonly IExamService _examService = new ExamService();
    private readonly ICourseService _courseService = new CourseService();
    private readonly IScheduleService _scheduleService = new ScheduleService();
    private readonly IStudentService _studentService = new StudentService();
    private readonly IMessageService _messageService = new MessageService();    
    private readonly IExamRepository _examRepository = new ExamFileRepository();

    public List<Teacher> GetAll()
    {
        return _userRepository.GetAll().OfType<Teacher>().ToList();
    }

    public List<Course> GetCourses(int teacherId)
    {
        return _courseRepository.GetAll().Where(course => course.TeacherId == teacherId).ToList();
    }

    public List<Exam> GetExams(int teacherId)
    {
        return _examService.GetAll().Where(exam => exam.TeacherId == teacherId).ToList();
    }

    public List<Teacher> GetAvailableTeachers(Course course)
    {
        List<Teacher> availableTeachers = new List<Teacher>();
        foreach (Teacher teacher_ in GetAll())
        {
            Course tempCourse = new Course(course.Language, course.Duration, course.Held, true,
                course.MaxStudents, course.CreatorId, course.ScheduledTime, course.StartDate,
                course.AreApplicationsClosed, teacher_.Id);

            if (_scheduleService.ValidateScheduleItem(tempCourse, true))
            {
                availableTeachers.Add(teacher_);
            }
        }

        return availableTeachers;
    }

    // delete all courses and exams that the teacher created
    // if they are on active course it can not be deleted
    // if they are on courses or exams that director chose, just remove them
    // ONLY DELETE EXAMS AND COURSES IN THE FUTURE
    public void Delete(int teacherId)
    {
        foreach(Course course in _courseRepository.GetAll())
        {
            if (course.TeacherId == teacherId && (DateTime.Now - course.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays >= 0 && course.Confirmed && !course.IsFinished)
            {
                throw new InvalidInputException("You cannot delete this teacher while they are on an active course.");
            }                                                                                         

            if (course.TeacherId == teacherId && course.StartDate.ToDateTime(TimeOnly.MinValue) > DateTime.Today)
            {
                // creator of the course is either teacher or director
                switch (_userRepository.GetById(course.CreatorId))
                {
                    case Teacher:
                        _courseService.Delete(course.Id);
                        break;
                    case Director:
                        course.TeacherId = null;
                        _courseRepository.Update(course);
                        break;
                }
            }
        }
        foreach(Exam exam in _examRepository.GetAll())
        {
            // if exam is in the future delete it
            if(exam.Date.ToDateTime(TimeOnly.MinValue) > DateTime.Today && exam.TeacherId == teacherId)
            {
                _examService.Delete(exam.Id);
            }    
        }
        _userRepository.Delete(teacherId);
    }

    public void RejectStudentApplication(int studentId, int courseId)
    {
        Course course = _courseRepository.GetById(courseId)!;

        if (!course.Students.ContainsKey(studentId))
            throw new InvalidInputException("Student hasn't applied to this course.");
        
        // course.RemoveStudent(studentId);
        course.Students[studentId] = ApplicationStatus.Denied;
        _courseRepository.Update(course);
    }
    
    public void ConfirmCourse(int courseId)
    {
        Course course = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");
        course.Confirmed = true;
        
        foreach ((int studentId, ApplicationStatus applicationStatus) in course.Students)
        {
            Student student = _userRepository.GetById(studentId) as Student ??
                              throw new InvalidInputException("Student doesn't exist.");

            switch (applicationStatus)
            {
                // All paused and denied applications are removed
                case ApplicationStatus.Paused:
                case ApplicationStatus.Denied:
                    course.RemoveStudent(studentId);
                    student.RemoveCourse(courseId);
                    _userRepository.Update(student);
                    if (applicationStatus == ApplicationStatus.Denied)
                        _messageService.Add(studentId, $"Your application for the course {course.Language.Name} has been denied.");
                    break;
                default:
                    student.SetActiveCourse(courseId);
                    student.RemoveCourse(courseId);
                    _studentService.PauseOtherApplications(studentId, courseId);
                    _userRepository.Update(student);
                    _messageService.Add(studentId, $"Your application for the course {course.Language.Name} has been accepted.");
                    break;
            }
        }
        
        _courseRepository.Update(course);
    }
    
    private void CheckGrades(int courseId)
    {
        Course course = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");

        foreach (int studentId in course.Students.Keys)
        {
            Student student = _userRepository.GetById(studentId) as Student ??
                              throw new InvalidInputException("Student doesn't exist.");

            if (!student.CourseGradeIds.ContainsKey(courseId))
            {
                throw new InvalidInputException("Not all students have been graded.");
            }
        }
    }
    
    public void FinishCourse(int courseId)
    {
        Course course = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");
        CheckGrades(courseId);
        course.IsFinished = true;
        _courseRepository.Update(course);
        
        foreach (int studentId in course.Students.Keys)
        {
            Student student = (_userRepository.GetById(studentId) as Student)!;
            student.LanguagePassFail[course.Language.Id] = false;
            // Course is dropped whe student reviews the teacher
            // student.DropActiveCourse();
            _studentService.ResumeApplications(studentId);
            _userRepository.Update(student);
        }
    }
    // get all available teachers and sort them based on ranking
    // pick the first one as the best choice
    public int? SmartPick(Course course)
    {
        List<Teacher> availableTeachers = GetAvailableTeachers(course)
            .OrderByDescending(teacher => teacher.Rating)
            .ToList();

        if (!availableTeachers.Any())
            throw new Exception("There are no available substitute teachers");

        course.TeacherId = availableTeachers.First().Id;
        availableTeachers.First().CourseIds.Add(course.Id);
        _courseRepository.Update(course);
        return course.TeacherId;
    }
}