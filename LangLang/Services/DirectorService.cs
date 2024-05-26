using LangLang.Repositories;
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

        private const string ReportsFolderName = "Reports";
        private const string LanguageReportSubfolder = "LanguageReports";

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
