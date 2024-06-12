using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using LangLang.Models;
using LangLang.Repositories;
using LangLang.Services;
using LangLang.Services.ReportServices;
using Microsoft.Extensions.DependencyInjection;

namespace LangLang.FormTable
{
    public class MainMenu
    {
        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            IUserService userService = serviceProvider.GetRequiredService<IUserService>();
            ITeacherService teacherService = serviceProvider.GetRequiredService<ITeacherService>();
            IExamService examService = serviceProvider.GetRequiredService<IExamService>();
            ICourseService courseService = serviceProvider.GetRequiredService<ICourseService>();

            User user = Loggin(userService);

            while (true)
            {
                switch (user)
                {
                    case Teacher:
                        TeacherMenu(examService, courseService);
                        break;
                    case Director:
                        DirectorMenu(teacherService);
                        break;
                    default:
                        break;
                }
            }

        }
        private static User? Loggin(IUserService userService)
        {
            while (true)
            {
                Console.WriteLine("Email >> ");
                string email = Console.ReadLine();
                Console.WriteLine("Password >> ");
                string password = Console.ReadLine();
                User? user = userService.Login(email!, password!);
                switch (user)
                {
                    case null:
                        Console.WriteLine("Error logging in. Try again? y/n");
                        string option = Console.ReadLine();
                        if (option == "n") return null;
                        continue;
                    case Teacher:
                        return user;
                    case Director:
                        return user;
                    default:
                        break;
                }
            }
        }
        private static void TeacherMenu(IExamService examService, ICourseService courseService)
        {
            Console.Write("" +
              "1) Create exams\n" +
              "2) Read exams\n" +
              "3) Update exams\n" +
              "4) Delete exams\n" +
              "5) Create courses\n" +
              "6) Read courses\n" +
              "7) Update courses\n" +
              "8) Delete courses\n" +
              "Enter option >> ");
            string option = Console.ReadLine();
            switch(option)
            {
                case "2": 
                    new FormTableGenerator<Exam,IExamService>(examService.GetAll()).ShowTable();
                    break;
                case "6":
                    new FormTableGenerator<Course,ICourseService>(courseService.GetAll()).ShowTable();
                    break;
                default: break;
            }
        }
        private static void DirectorMenu(ITeacherService teacherService)
        {
            Console.Write("" +
                "1) Create teachers\n" +
                "2) Read teachers\n" +
                "3) Update teachers\n" +
                "4) Delete teachers\n" +
                "5) Create courses - smart pick\n" +
                "6) Create exams - smart pick\n" +
                "Enter option >> ");
            string option = Console.ReadLine();
            switch (option)
            {
                case "2":
                    new FormTableGenerator<Teacher,ITeacherService>(teacherService.GetAll()).ShowTable();
                    break;
                default: break;
            }
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
        }
    }
}
