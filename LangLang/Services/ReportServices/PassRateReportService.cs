using LangLang.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LangLang.Models;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot;

namespace LangLang.Services.ReportServices
{
    public class PassRateReportService : IPassRateReportService
    {

        private readonly IExamRepository _examRepository;
        private readonly IExamGradeService _examGradeService;
        private const string ReportsFolderName = "Reports";
        private const string PassRateReportSubfolder = "PassRateReports";
        public PassRateReportService(IExamRepository examRepository, IExamGradeService examGradeService) {
            _examRepository = examRepository;
            _examGradeService = examGradeService;
        }
        public void GeneratePointsPassRateReport()
        {
            Directory.CreateDirectory(Path.Combine(ReportsFolderName, PassRateReportSubfolder));

            List<double> pointsAvg = AveragePointsInLastYear();

            PlotModel gradeAvgPlotModel = CreatePointsAvgPlotModel(pointsAvg);

            string pointsAvgPath = Path.Combine(ReportsFolderName, PassRateReportSubfolder, DateTime.Now.ToString("yyyy-MMMM-dd-hh-mm") + ".pdf");
            SaveToPdf(gradeAvgPlotModel, pointsAvgPath);

            EmailService.SendMessage("PassRate report", "Today's PassRate report is attached in this email", pointsAvgPath);
        }
        private static PlotModel CreatePointsAvgPlotModel(List<double> data)
        {
            var model = new PlotModel { Title = "Average points on each part of the exam report" };
            var series = new HistogramSeries();
            series.Items.Add(new HistogramItem(-0.25, 0.25, Math.Round(data[0]) / 2, 1));
            series.Items.Add(new HistogramItem(0.75, 1.25, Math.Round(data[1]) / 2, 1));
            series.Items.Add(new HistogramItem(1.75, 2.25, Math.Round(data[2]) / 2, 1));
            series.Items.Add(new HistogramItem(2.75, 3.25, Math.Round(data[3]) / 2, 1));

            model.Series.Add(series);

            var xAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Grades"
            };
            xAxis.Labels.Add("ListeningPoints");
            xAxis.Labels.Add("TalkingPoints");
            xAxis.Labels.Add("WritingPoints");
            xAxis.Labels.Add("ReadingPoints");
            model.Axes.Add(xAxis);

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Average" });

            return model;
        }
        private static void SaveToPdf(PlotModel plotModel, string filePath)
        {
            using var stream = File.Create(filePath);
            var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
            pdfExporter.Export(plotModel, stream);
        }
        /*
      Prosečan broj poena ostvaren na svakom od delova svih ispita u poslednjih
      godinu dana. 
      Koliko je studenata slušalo kurs, a koliko položilo, pored toga
      navesti i procenat studenata koji je položio u odnosu na one koje je slušao
  */
        public List<double> AveragePointsInLastYear()
        {
            List<double> sumOfPoints = new List<double> { 0.0, 0.0, 0.0, 0.0 };
            int gradeCount = 0;

            DateTime oneYearAgo = DateTime.Today.AddYears(-1);

            foreach (Exam exam in _examRepository.GetAll())
            {
                if (exam.Date.ToDateTime(TimeOnly.MinValue) >= oneYearAgo && exam.TeacherGraded)
                {
                    foreach (ExamGrade grade in _examGradeService.GetByExamId(exam.Id))
                    {
                        sumOfPoints[0] += grade.ListeningPoints;
                        sumOfPoints[1] += grade.TalkingPoints;
                        sumOfPoints[2] += grade.WritingPoints;
                        sumOfPoints[3] += grade.ReadingPoints;
                        gradeCount++;
                    }
                }
            }

            if (gradeCount > 0)
            {
                for (int i = 0; i < sumOfPoints.Count; i++)
                {
                    sumOfPoints[i] /= gradeCount;
                }
            }

            return sumOfPoints;
        }
    }
}
