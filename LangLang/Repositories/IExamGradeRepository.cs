using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repositories
{
    public interface IExamGradeRepository
    {
        public List<ExamGrade> GetAll();
        public void Add(ExamGrade examGrade);
        public void Update(ExamGrade examGrade);
        public void Delete(int id);
        public int GenerateId();
    }
}
