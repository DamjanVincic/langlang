using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services;

public class TeacherService : ITeacherService
{
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;
    
    private readonly IExamService _examService;
    private readonly IScheduleService _scheduleService;
    private readonly IStudentService _studentService;
    private readonly IMessageService _messageService;

    public TeacherService(IUserRepository userRepository, ICourseRepository courseRepository, IExamService examService, IScheduleService scheduleService, IStudentService studentService, IMessageService messageService)
    {
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _examService = examService;
        _scheduleService = scheduleService;
        _studentService = studentService;
        _messageService = messageService;
    }

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
}