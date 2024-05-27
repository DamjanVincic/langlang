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
        private const string ReportsFolderName = "Reports";
        private const string LanguageReportSubfolder = "LanguageReports";
        
        // TODO: Implement dependency injection
        private readonly IUserRepository _userRepository = new UserFileRepository();
        private readonly IPenaltyPointRepository _penaltyPointRepository = new PenaltyPointFileRepository();
        private readonly ICourseGradeRepository _courseGradeRepository = new CourseGradeFileRepository();
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();
        private readonly IExamRepository _examRepository = new ExamFileRepository();
        private readonly ILanguageRepository _languageRepository = new LanguageFileRepository();
        private readonly IExamGradeRepository _examGradeRepository = new ExamGradeFileRepository();

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

        public void GenerateLanguageReport()
        {
            Directory.CreateDirectory(Path.Combine(ReportsFolderName, LanguageReportSubfolder));

            Dictionary<int, double> courseCount = GetCourseCount();

            PlotModel courseCountPlotModel = CreateLanguagePlotModel("Course count", courseCount);

            string courseCountPath = Path.Combine(ReportsFolderName, LanguageReportSubfolder, "courseCount.pdf");
            SaveToPdf(courseCountPlotModel, courseCountPath);

            Dictionary<int, double> examCount = GetExamCount();

            PlotModel examCountPlotModel = CreateLanguagePlotModel("Exam count", examCount);

            string examCountPath = Path.Combine(ReportsFolderName, LanguageReportSubfolder, "examCount.pdf");
            SaveToPdf(examCountPlotModel, examCountPath);

            Dictionary<int, double> penaltyAvg = GetPenaltyAvg();

            PlotModel penaltyAvgPlotModel = CreateLanguagePlotModel("Penalty point average", penaltyAvg);

            string penaltyAvgPath = Path.Combine(ReportsFolderName, LanguageReportSubfolder, "penaltyAvg.pdf");
            SaveToPdf(penaltyAvgPlotModel, penaltyAvgPath);

            Dictionary<int, double> examGradeAvg = GetExamGradeAvg();

            PlotModel examGradeAvgPlotModel = CreateLanguagePlotModel("Exam Grade average", examGradeAvg);

            string examGradeAvgPath = Path.Combine(ReportsFolderName, LanguageReportSubfolder, "examGradeAvg.pdf");
            SaveToPdf(examGradeAvgPlotModel, examGradeAvgPath);

            string reportPath = Path.Combine(ReportsFolderName, LanguageReportSubfolder,
                DateTime.Now.ToString("yyyy-MMMM-dd-hh-mm") + ".pdf");

            MergePdf(reportPath, new[] { courseCountPath, examCountPath, penaltyAvgPath, examGradeAvgPath });

            EmailService.SendMessage("Language report","Today's language report is attached in this email",reportPath);
        }

        private Dictionary<int, double> GetExamCount()
        {
            // LanguageId, Exam count
            Dictionary<int, double> examCount = new();

            foreach (Exam exam in _examRepository.GetAll())
            {
                if ((DateTime.Now - exam.Date.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;

                if (!examCount.TryAdd(exam.Language.Id, 1))
                    examCount[exam.Language.Id] += 1;
            }

            return examCount;
        }

        private Dictionary<int, double> GetCourseCount()
        {
            // LanguageId, Course count
            Dictionary<int, double> courseCount = new();

            foreach (Course course in _courseRepository.GetAll())
            {
                if ((DateTime.Now - course.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;

                if (!courseCount.TryAdd(course.Language.Id, 1))
                    courseCount[course.Language.Id] += 1;
            }

            return courseCount;
        }

        private Dictionary<int, double> GetPenaltyAvg()
        {
            Dictionary<int, int> coursePenaltyCount = new();

            foreach (PenaltyPoint penaltyPoint in _penaltyPointRepository.GetAll())
            {
                if ((DateTime.Now - penaltyPoint.Date.ToDateTime(TimeOnly.MinValue)).TotalDays >
                    365) continue;

                if (!coursePenaltyCount.TryAdd(penaltyPoint.CourseId, 1))
                    coursePenaltyCount[penaltyPoint.CourseId] += 1;
            }

            Dictionary<int, int> languagePenaltyCount = new();
            Dictionary<int, int> languageCourseCount = new();

            foreach (int courseId in coursePenaltyCount.Keys)
            {
                Course course = _courseRepository.GetById(courseId)!;

                if (!languagePenaltyCount.TryAdd(course.Language.Id, coursePenaltyCount[courseId]))
                    languagePenaltyCount[course.Language.Id] += coursePenaltyCount[courseId];

                if (!languageCourseCount.TryAdd(course.Language.Id, 1))
                    languageCourseCount[course.Language.Id] += 1;
            }

            Dictionary<int, double> penaltyAvg = new();

            foreach (int languageId in languagePenaltyCount.Keys)
            {
                penaltyAvg[languageId] = (double)languagePenaltyCount[languageId] / languageCourseCount[languageId];
            }

            return penaltyAvg;
        }

        private Dictionary<int, double> GetExamGradeAvg()
        {
            Dictionary<int, int> languageGradeCount = new();
            Dictionary<int, int> languageGradeSum = new();

            foreach (ExamGrade examGrade in _examGradeRepository.GetAll())
            {
                Exam exam = _examRepository.GetById(examGrade.ExamId)!;

                if ((DateTime.Now - exam.Date.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) 
                    continue;

                if (!languageGradeCount.TryAdd(exam.Language.Id, 1))
                    languageGradeCount[exam.Language.Id] += 1;

                if (!languageGradeSum.TryAdd(exam.Language.Id, examGrade.PointsSum))
                    languageGradeSum[exam.Language.Id] += examGrade.PointsSum;
            }

            Dictionary<int, double> examGradeAvg = new();

            foreach (int languageId in languageGradeSum.Keys)
            {
                examGradeAvg[languageId] = (double)languageGradeSum[languageId] / languageGradeCount[languageId];
            }

            return examGradeAvg;
        }
        
        private PlotModel CreateLanguagePlotModel(string title, Dictionary<int,double> data)
        {
            var plotModel = new PlotModel();
            plotModel.Title = title;

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.AddRange(data.Keys.Select(id => _languageRepository.GetById(id)!.ToString()).ToList());

            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };

            var barSeries = new BarSeries {StrokeColor = OxyColors.Black, StrokeThickness = 1 };
            barSeries.ActualItems.AddRange(data.Values.Select(value => new BarItem { Value = value }).ToList());

            plotModel.Series.Add(barSeries);
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);

            return plotModel;
        }

        private static void SaveToPdf(PlotModel plotModel, string filePath)
        {
            using (var stream = File.Create(filePath))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(plotModel, stream);
            }
        }
        private static void MergePdf(string outputFilePath, string[] inputFilePaths)
        {
            PdfDocument outputPdfDocument = new PdfDocument();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            foreach (string filePath in inputFilePaths)
            {
                PdfDocument inputPdfDocument = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
                outputPdfDocument.Version = inputPdfDocument.Version;
                foreach (PdfPage page in inputPdfDocument.Pages)
                {
                    outputPdfDocument.AddPage(page);
                }
            }
            outputPdfDocument.Save(outputFilePath);

            foreach (string filePath in inputFilePaths)
            {
                File.Delete(filePath);
            }
        }
    }
}
