using LangLang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services
{
    public interface ICourseGradeService
    {
        public List<CourseGrade> GetAll();
        public CourseGrade? GetById(int id);
        public int Add(int courseId, int studentId, int knowledgeGrade, int activityGrade);
        public void Delete(int id);
    }
}
