using System.Collections.Generic;
using LangLang.Models;

namespace LangLang.Repositories
{
    public interface IExamGradeRepository
    {
        public List<ExamGrade> GetAll();
        public void Add(ExamGrade examGrade);
        public void Update(ExamGrade examGrade);
        public void Delete(int id);
        public int GenerateId();
        public ExamGrade GetById(int id);
    }
}
