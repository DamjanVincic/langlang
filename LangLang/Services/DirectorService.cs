using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly IUserRepository _userRepository = new UserFileRepository();
        private readonly IPenaltyPointRepository _penaltyPointRepository = new PenaltyPointFileRepository();
        private readonly ICourseGradeRepository _courseGradeRepository = new CourseGradeFileRepository();

        // Number of penalties on every course in the last year
        // Average number of points for students with 0 through 3 penalties
        public void GeneratePenaltyReport()
        {
            // courseId, penaltyCount
            Dictionary<int, int> coursePenalties = new();
            
            // numberOfPenalties (0 - 3), (pointType, pointCount)
            Dictionary<int, Dictionary<string, double>> averageStudentPoints = new();
            
            // numberOfPenalties (0 - 3), studentCount
            Dictionary<int, int> totalStudents = new();
            
            foreach (PenaltyPoint penaltyPoint in _penaltyPointRepository.GetAll())
            {
                if ((DateTime.Now - penaltyPoint.Date.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;
                
                if (!coursePenalties.TryAdd(penaltyPoint.CourseId, 1))
                    coursePenalties[penaltyPoint.CourseId] += 1;
            }

            foreach (Student student in _userRepository.GetAll().OfType<Student>())
            {
                int numberOfPenalties = _penaltyPointRepository.GetAll().Count(point => point.StudentId == student.Id);
                List<CourseGrade> courseGrades = _courseGradeRepository.GetAll().Where(grade => grade.StudentId == student.Id).ToList();
                
                double averageKnowledgeGrade = 0, averageActivityGrade = 0;
                
                foreach (CourseGrade courseGrade in courseGrades)
                {
                    averageKnowledgeGrade += courseGrade.KnowledgeGrade;
                    averageActivityGrade += courseGrade.ActivityGrade;
                }
                
                if (courseGrades.Count > 0)
                {
                    averageKnowledgeGrade /= courseGrades.Count;
                    averageActivityGrade /= courseGrades.Count;
                }
                
                if (!averageStudentPoints.TryAdd(numberOfPenalties, new Dictionary<string, double> { { "knowledgeGrade", 0 }, { "activityGrade", 0 } }))
                {
                    averageStudentPoints[numberOfPenalties]["knowledgeGrade"] += averageKnowledgeGrade;
                    averageStudentPoints[numberOfPenalties]["activityGrade"] += averageActivityGrade;
                }
                
                if (!totalStudents.TryAdd(numberOfPenalties, 1))
                    totalStudents[numberOfPenalties] += 1;
            }
            
            foreach ((int penaltyCount, Dictionary<string, double> pointTypes) in averageStudentPoints)
            {
                pointTypes["knowledgeGrade"] /= totalStudents[penaltyCount];
                pointTypes["activityGrade"] /= totalStudents[penaltyCount];
            }
            
            // Send the report
        }
    }
}
