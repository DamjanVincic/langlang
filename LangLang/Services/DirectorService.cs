using LangLang.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LangLang.Models;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot;
using OxyPlot.WindowsForms;
using PdfSharp.Pdf.IO;
using Element = iTextSharp.text.Element;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using PdfPage = PdfSharp.Pdf.PdfPage;
using PdfReader = PdfSharp.Pdf.IO.PdfReader;

namespace LangLang.Services
{
    public class DirectorService : IDirectorService
    {
        private const int NumberOfTopStudents = 3;
        
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICourseGradeRepository _courseGradeRepository;
        private readonly ICourseGradeService _courseGradeService;
        private readonly IExamRepository _examRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IPenaltyPointRepository _penaltyPointRepository;
        private readonly IExamGradeRepository _examGradeRepository;
        
        public DirectorService(ICourseRepository courseRepository, IUserRepository userRepository, ICourseGradeRepository courseGradeRepository,
            ICourseGradeService courseGradeService, IExamRepository examRepository, ILanguageRepository languageRepository,
            IPenaltyPointRepository penaltyPointRepository, IExamGradeRepository examGradeRepository)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _courseGradeRepository = courseGradeRepository;
            _courseGradeService = courseGradeService;
            _examRepository = examRepository;
            _languageRepository = languageRepository;
            _penaltyPointRepository = penaltyPointRepository;
            _examGradeRepository = examGradeRepository;
        }

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
        
        private static void GeneratePenaltyReportPdf(Dictionary<Course, int> coursePenalties, Dictionary<int, Dictionary<string, double>> averageStudentPoints, string filePath = "PenaltyReport.pdf")
        {
            Document document = new Document();

            using FileStream stream = new FileStream(filePath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            document.Open();

            Paragraph title = new Paragraph("Penalty Report", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            document.Add(new Paragraph("Course Penalties:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))
            {
                SpacingAfter = 10
            });

            document.Add(GeneratePenaltyTable(coursePenalties));

            document.Add(new Paragraph("Average Student Grades:", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD))
            {
                SpacingAfter = 10
            });

            document.Add(GenerateAverageStudentPointsTable(averageStudentPoints));

            var plotModel = GenerateStudentAveragePointsChart(averageStudentPoints);
            byte[] chartBytes = RenderChartAsImage(plotModel);
            
            Image image = Image.GetInstance(chartBytes);
            document.Add(image);

            document.Close();
            writer.Close();
        }

        private static PdfPTable GenerateAverageStudentPointsTable(Dictionary<int, Dictionary<string, double>> averageStudentPoints)
        {
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

            return averageStudentPointsTable;
        }

        private static PdfPTable GeneratePenaltyTable(Dictionary<Course, int> coursePenalties)
        {
            PdfPTable penaltyTable = new PdfPTable(4)
            {
                SpacingAfter = 30
            };

            penaltyTable.AddCell(
                new PdfPCell(new Phrase("Course ID", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            penaltyTable.AddCell(
                new PdfPCell(new Phrase("Language", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            penaltyTable.AddCell(
                new PdfPCell(new Phrase("Start Date", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            penaltyTable.AddCell(
                new PdfPCell(new Phrase("Number of Penalties", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)))
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

            return penaltyTable;
        }

        private static PlotModel GenerateStudentAveragePointsChart(Dictionary<int, Dictionary<string, double>> averageStudentPoints)
        {
            var plotModel = new PlotModel { Title = "Average Student Grades by Penalty Count" };
            
            var knowledgeSeries = new BarSeries { Title = "Average Knowledge Grade" };
            var activitySeries = new BarSeries { Title = "Average Activity Grade" };
            
            foreach (var (numberOfPenaltyPoints, averagePoints) in averageStudentPoints)
            {
                knowledgeSeries.Items.Add(new BarItem(averagePoints["knowledgeGrade"], numberOfPenaltyPoints));
                activitySeries.Items.Add(new BarItem(averagePoints["activityGrade"], numberOfPenaltyPoints));
            }
            
            plotModel.Series.Add(knowledgeSeries);
            plotModel.Series.Add(activitySeries);
            
            plotModel.Axes.Add(new CategoryAxis { Position = AxisPosition.Left, Title = "Penalty Point Count", ItemsSource = new List<int> {0, 1, 2, 3}});
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Average Grade" });

            return plotModel;
        }

        private static byte[] RenderChartAsImage(PlotModel plotModel)
        {
            using MemoryStream stream = new MemoryStream();
            var pngExporter = new PngExporter { Width = 500, Height = 400 };
            pngExporter.Export(plotModel, stream);
            return stream.ToArray();
        }

        // knowledgePoints - if true they have priority over activity points
        public void NotifyBestStudents(int courseId, bool knowledgePoints)
        {
            double knowledgePointsMultiplier = knowledgePoints ? 1.5 : 1;
            double activityPointsMultiplier = knowledgePoints ? 1 : 1.5;
            double penaltyMultiplier = 2.0;

            Course course = _courseRepository.GetById(courseId)!;

            // student, calculatedPoints
            Dictionary<Student, double> rankedStudents = new();

            foreach (Student student in course.Students.Keys.Select(studentId => (_userRepository.GetById(studentId) as Student)!))
            {
                CourseGrade courseGrade = _courseGradeService.GetByStudentAndCourse(student.Id, courseId)!;
                double studentPoints = courseGrade.KnowledgeGrade * knowledgePointsMultiplier + courseGrade.ActivityGrade * activityPointsMultiplier - student.PenaltyPoints * penaltyMultiplier;
                rankedStudents.Add(student, studentPoints);
            }

            List<Student> bestStudents = rankedStudents.OrderByDescending(pair => pair.Value).Take(NumberOfTopStudents).Select(pair => pair.Key).ToList();

            foreach (Student student in bestStudents)
            {
                EmailService.SendMessage($"Congratulations {student.FirstName} {student.LastName}!",
                    $"You are one of the best students in the course {course.Language.Name} {course.Language.Level}.");
            }

            course.StudentsNotified = true;
            _courseRepository.Update(course);
        }
    }
}
