using LangLang.Models;
using LangLang.Repositories;
using System.Collections.Generic;

namespace LangLang.Services
{
    internal class CourseGradeService : ICourseGradeService
    {
        private readonly ICourseGradeRepository _courseGradeRepository = new CourseGradeFileRepository();
        private readonly IUserRepository _userRepository = new UserFileRepository();
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();

        public List<CourseGrade> GetAll()
        {
            return _courseGradeRepository.GetAll();
        }

        public CourseGrade? GetById(int id)
        {
            return _courseGradeRepository.GetById(id);
        }

        public int Add(int courseId, int studentId, int knowledgeGrade, int activityGrade)
        {
            _ = _userRepository.GetById(studentId) as Student ??
                              throw new InvalidInputException("User doesn't exist.");
            _ = _courseRepository.GetById(courseId) ?? throw new InvalidInputException("Course doesn't exist.");

            CourseGrade courseGrade = new(courseId, studentId, knowledgeGrade, activityGrade) { Id = _courseGradeRepository.GenerateId() };

            _courseGradeRepository.Add(courseGrade);

            return courseGrade.Id;
        }

        public void Delete(int id)
        {
            _courseGradeRepository.Delete(id);
        }
    }
}
