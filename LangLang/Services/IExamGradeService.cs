using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Services
{
    public interface IExamGradeService
    {
        public List<ExamGrade> GetAll();
        public ExamGrade? GetById(int id);
        public void Add(int examId, int studentId, int readingPoints, int writingPoints, int listeningPoints, int talkingPoints);
        public void Delete(int id);
    }
}
