using System.Collections.Generic;
using System.Linq;
using LangLang.Model;
using LangLang.Repositories;

namespace LangLang.Services;

public class TeacherService : ITeacherService
{
    private readonly IUserRepository _userRepository = new UserFileRepository();
    private readonly IExamService _examService = new ExamService();
    
    public List<Teacher> GetAll()
    {
        return _userRepository.GetAll().OfType<Teacher>().ToList();
    }
    
    public List<Exam> GetExams(int teacherId)
    {
        return _examService.GetAll().Where(exam => exam.TeacherId == teacherId).ToList();
    }
}