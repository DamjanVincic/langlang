using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Model;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services
{
    internal class ExamGradeService : IExamGradeService
    {
        private readonly IExamGradeRepository _examGradeRepository = new ExamGradeFileRepository();
        private readonly IUserRepository _userRepository = new UserFileRepository();
        private readonly IExamRepository _examRepository = new ExamFileRepository();

        public List<ExamGrade> GetAll()
        {
            return _examGradeRepository.GetAll();
        }

        public ExamGrade? GetById(int id)
        {
            return _examGradeRepository.GetById(id);
        }

        public void Add(int examId, int studentId, int readingPoints, int writingPoints, int listeningPoints,
            int talkingPoints)
        {
            Student student = _userRepository.GetById(studentId) as Student ??
                              throw new InvalidInputException("User doesn't exist.");
            Exam exam = _examRepository.GetById(examId) ?? throw new InvalidInputException("Exam doesn't exist.");

            ExamGrade examGrade = new ExamGrade(_examGradeRepository.GenerateId(), examId, studentId, readingPoints,
                writingPoints, listeningPoints, talkingPoints);
            
            _examGradeRepository.Add(examGrade);
        }

        public void Delete(int id)
        {
            _examGradeRepository.Delete(id);
        }
    }
}
