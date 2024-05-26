﻿using LangLang.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Effects;
using LangLang.Models;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot;
using OxyPlot.Legends;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

namespace LangLang.Services
{
    public class DirectorService:IDirectorService
    {
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();
        private readonly IExamRepository _examRepository = new ExamFileRepository();
        private readonly ILanguageRepository _languageRepository = new LanguageFileRepository();
        private readonly IPenaltyPointRepository _penaltyPointRepository = new PenaltyPointFileRepository();

        private const string ReportsFolderName = "Reports";
        private const string LanguageReportSubfolder = "LanguageReports";

        public void GenerateLanguageReport()
        {
            Directory.CreateDirectory(Path.Combine(ReportsFolderName,LanguageReportSubfolder));

            Dictionary<int, int> courseCount = GetCourseCount();

            PlotModel courseCountPlotModel = createLanguagePlotModel("Course count", courseCount);

            string courseCountPath = Path.Combine(ReportsFolderName, LanguageReportSubfolder, "courseCount.pdf");
            SaveToPdf(courseCountPlotModel, courseCountPath);

            Dictionary<int, int> examCount = GetExamCount();

            PlotModel examCountPlotModel = createLanguagePlotModel("Exam count", examCount);

            string examCountPath = Path.Combine(ReportsFolderName, LanguageReportSubfolder, "examCount.pdf");
            SaveToPdf(examCountPlotModel, examCountPath);

            string reportPath= Path.Combine(ReportsFolderName, LanguageReportSubfolder, DateTime.Now.ToString("yyyy-MMMM-dd-hh-mm")+".pdf");

            MergePdf(reportPath, new[] { courseCountPath, examCountPath });
        }

        private Dictionary<int, int> GetExamCount()
        {
            // LanguageId, Exam count
            Dictionary<int, int> examCount = new();

            foreach (Exam exam in _examRepository.GetAll())
            {
                if ((DateTime.Now - exam.Date.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;

                if (!examCount.TryAdd(exam.Language.Id, 1))
                    examCount[exam.Language.Id] += 1;
            }

            return examCount;
        }

        private Dictionary<int, int> GetCourseCount()
        {
            // LanguageId, Course count
            Dictionary<int, int> courseCount = new();

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
            Dictionary<int,int> coursePenaltyCount = new();

            foreach (PenaltyPoint penaltyPoint in _penaltyPointRepository.GetAll())
            {
                if ((DateTime.Now - penaltyPoint.DatePenaltyPointGiven.ToDateTime(TimeOnly.MinValue)).TotalDays > 365) continue;

                if (!coursePenaltyCount.TryAdd(penaltyPoint.CourseId, 1))
                    coursePenaltyCount[penaltyPoint.CourseId] += 1;
            }

            Dictionary<int, int> languagePenaltyCount = new();
            Dictionary<int, int> languageCourseCount = new();

            foreach (int courseId in coursePenaltyCount.Keys)
            {
                Course course = _courseRepository.GetById(courseId);

                if (!languagePenaltyCount.TryAdd(course.Language.Id, coursePenaltyCount[courseId]))
                    languagePenaltyCount[course.Language.Id] += coursePenaltyCount[courseId];

                if (!languageCourseCount.TryAdd(course.Language.Id, 1))
                    languageCourseCount[course.Language.Id] += 1;
            }

            Dictionary<int, double> penaltyAvg = new();

            foreach (int languageId in languagePenaltyCount.Keys)
            {
                penaltyAvg[languageId]=languagePenaltyCount[languageId]/languageCourseCount[languageId];
            }

            return penaltyAvg;
        }



        private PlotModel createLanguagePlotModel(string title, Dictionary<int, int> data)
        {
            var plotModel = new PlotModel();
            plotModel.Title = title;

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.AddRange(data.Keys.Select(id => _languageRepository.GetById(id).ToString()).ToList());

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
            PdfDocument outputPDFDocument = new PdfDocument();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            foreach (string filePath in inputFilePaths)
            {
                PdfDocument inputPDFDocument = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);
                outputPDFDocument.Version = inputPDFDocument.Version;
                foreach (PdfPage page in inputPDFDocument.Pages)
                {
                    outputPDFDocument.AddPage(page);
                }
            }
            outputPDFDocument.Save(outputFilePath);

            foreach (string filePath in inputFilePaths)
            {
                File.Delete(filePath);
            }
        }
    }
}
