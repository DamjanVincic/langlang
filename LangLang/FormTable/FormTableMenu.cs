using System;
using GalaSoft.MvvmLight;
using LangLang.Models;
using LangLang.Services;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using LangLang.Repositories;
using LangLang.Services.ReportServices;
using Microsoft.Extensions.DependencyInjection;

namespace LangLang.FormTable
{
    public class FormTableMenu
    {
        private readonly ICourseService _courseService;
        private readonly ILanguageService _languageService;
        private readonly CourseFormTable _courseFormTable;

        public FormTableMenu(ICourseService courseService, ILanguageService languageService)
        {
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _languageService = languageService;
            _courseFormTable = new CourseFormTable(_courseService,_languageService);
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("1) Teacher - CRUD course \n" +
                                  "2) Teacher - CRUD exam \n" +
                                  "3) Director - CRUD teacher \n" +
                                  "4) Director - CRUD/Smart Pick course \n" +
                                  "5) Director - CRUD/Smart Pick exam \n" +
                                  "q)uit");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        _courseFormTable.CourseMenu();
                        break;
                    case "q":
                    case "Q":
                        return;
                }
            }
        }

        static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var formTableMenu = serviceProvider.GetRequiredService<FormTableMenu>();
            formTableMenu.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICourseGradeRepository, CourseGradeFileRepository>();
            services.AddScoped<ICourseRepository, CourseFileRepository>();
            services.AddScoped<IExamGradeRepository, ExamGradeFileRepository>();
            services.AddScoped<IExamRepository, ExamFileRepository>();
            services.AddScoped<ILanguageRepository, LanguageFileRepository>();
            services.AddScoped<IMessageRepository, MessageFileRepository>();
            services.AddScoped<IPenaltyPointRepository, PenaltyPointFileRepository>();
            services.AddScoped<IScheduleRepository, ScheduleFileRepository>();
            services.AddScoped<IUserRepository, UserFileRepository>();

            services.AddScoped<ICourseGradeService, CourseGradeService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IDirectorService, DirectorService>();
            services.AddScoped<IExamGradeService, ExamGradeService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPenaltyPointService, PenaltyPointService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGradeReportService, GradeReportService>();
            services.AddScoped<IPassRateReportService, PassRateReportService>();
            services.AddScoped<ILanguageReportService, LanguageReportService>();

            services.AddTransient<FormTableMenu>();
        }
    }
}
