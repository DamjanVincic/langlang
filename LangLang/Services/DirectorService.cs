using LangLang.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Models;

namespace LangLang.Services
{
    internal class DirectorServices:IDirectorService
    {
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();
        private readonly IExamRepository _examRepository = new ExamFileRepository();

        public void GenerateLanguageReport()
        {
            // LanguageId, Course count
            Dictionary<int, int> courseCount = new();

            foreach (Course course in _courseRepository.GetAll())
            {
                if ((DateTime.Now - course.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;

                if (!courseCount.TryAdd(course.Language.Id, 1))
                    courseCount[course.Language.Id] += 1;
            }

            // LanguageId, Exam count
            Dictionary<int, int> examCount = new();

            foreach (Exam exam in _examRepository.GetAll())
            {
                if ((DateTime.Now - exam.Date.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;

                if (!examCount.TryAdd(exam.Language.Id, 1))
                    courseCount[exam.Language.Id] += 1;
            }
        }
    }
}
