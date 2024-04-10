using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.Services;

public interface ITeacherService
{
    // TODO: Implement course and exam creation etc.
    public List<Teacher> GetAll();
    public List<Exam> GetExams(int teacherId);
}