using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LangLang.Models;
using LangLang.Repositories;

namespace LangLang.Services
{
    public class DirectorService : IDirectorService
    {
        // TODO: Implement dependency injection
        private readonly IUserRepository _userRepository = new UserFileRepository();
        private readonly IPenaltyPointRepository _penaltyPointRepository = new PenaltyPointFileRepository();
        private readonly ICourseGradeRepository _courseGradeRepository = new CourseGradeFileRepository();
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();

        // Number of penalties on every course in the last year
        // Average number of points for students with 0 through 3 penalties
        public void GeneratePenaltyReport()
        {
            // course, penaltyCount
            Dictionary<Course, int> coursePenalties = new();
            
            // numberOfPenalties (0 - 3), (pointType, pointCount)
            Dictionary<int, Dictionary<string, double>> averageStudentPoints = new();
            
            // numberOfPenalties (0 - 3), studentCount
            Dictionary<int, int> totalStudents = new();
            
            foreach (PenaltyPoint penaltyPoint in _penaltyPointRepository.GetAll())
            {
                if ((DateTime.Now - penaltyPoint.Date.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;
                
                Course course = _courseRepository.GetById(penaltyPoint.CourseId)!;
                
                if (!coursePenalties.TryAdd(course, 1))
                    coursePenalties[course] += 1;
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
                
                if (!averageStudentPoints.TryAdd(numberOfPenalties, new Dictionary<string, double> { { "knowledgeGrade", averageKnowledgeGrade }, { "activityGrade", averageActivityGrade } }))
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
            string filePath = "PenaltyReport.pdf";
            GeneratePenaltyReportPdf(coursePenalties, averageStudentPoints, filePath);
            
            EmailService.SendMessage("Penalty Report", "Penalty report is attached.", filePath);
        }
        
        private void GeneratePenaltyReportPdf(Dictionary<Course, int> coursePenalties, Dictionary<int, Dictionary<string, double>> averageStudentPoints, string filePath = "PenaltyReport.pdf")
        {
            Document document = new Document();

            using FileStream stream = new FileStream("PenaltyReport.pdf", FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            document.Open();

            Paragraph title = new Paragraph(filePath, new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            document.Add(new Paragraph("Course Penalties:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))
            {
                SpacingAfter = 10
            });
                
            PdfPTable penaltyTable = new PdfPTable(4)
            {
                SpacingAfter = 30
            };

            penaltyTable.AddCell(new PdfPCell(new Phrase("Course ID", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            penaltyTable.AddCell(new PdfPCell(new Phrase("Language", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            penaltyTable.AddCell(new PdfPCell(new Phrase("Start Date", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            penaltyTable.AddCell(new PdfPCell(new Phrase("Number of Penalties", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
                
            foreach (var (course, numberOfPenaltyPoints) in coursePenalties)
            {
                penaltyTable.AddCell(new PdfPCell(new Phrase(course.Id.ToString()))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                penaltyTable.AddCell(new PdfPCell(new Phrase($"{course.Language.Name} {course.Language.Level}"))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                penaltyTable.AddCell(new PdfPCell(new Phrase(course.StartDate.ToShortDateString()))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                penaltyTable.AddCell(new PdfPCell(new Phrase(numberOfPenaltyPoints.ToString()))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }
                
            document.Add(penaltyTable);

            document.Add(new Paragraph("Average Student Grades:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))
            {
                SpacingAfter = 10
            });
                
            PdfPTable averageStudentPointsTable = new PdfPTable(3)
            {
                SpacingAfter = 30
            };
                
            averageStudentPointsTable.AddCell(new PdfPCell(new Phrase("Penalty Count", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            averageStudentPointsTable.AddCell(new PdfPCell(new Phrase("Knowledge Grade", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            averageStudentPointsTable.AddCell(new PdfPCell(new Phrase("Activity Grade", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
                
            foreach (var (numberOfPenaltyPoints, averagePoints) in averageStudentPoints)
            {
                averageStudentPointsTable.AddCell(new PdfPCell(new Phrase(numberOfPenaltyPoints.ToString()))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                averageStudentPointsTable.AddCell(new PdfPCell(new Phrase(averagePoints["knowledgeGrade"].ToString(CultureInfo.CurrentCulture)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                averageStudentPointsTable.AddCell(new PdfPCell(new Phrase(averagePoints["activityGrade"].ToString(CultureInfo.CurrentCulture)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }

            document.Add(averageStudentPointsTable);

            document.Close();
            writer.Close();
        }
    }
}
