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
        static User user;
        static void Main()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            IUserService userService = serviceProvider.GetRequiredService<IUserService>();
            ITeacherService teacherService = serviceProvider.GetRequiredService<ITeacherService>();
            IExamService examService = serviceProvider.GetRequiredService<IExamService>();
            ICourseService courseService = serviceProvider.GetRequiredService<ICourseService>();

            user = Loggin(userService);

            while (true)
            {
                switch (user)
                {
                    case Teacher:
                        TeacherMenu(examService, courseService);
                        break;
                    case Director:
                        DirectorMenu(teacherService,userService,courseService,examService);
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
            try
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
                  "9) Logout\n" +
                  "Enter option >> ");
                string option = Console.ReadLine();
                switch (option)
                {
                    // radi
                    case "1":
                        new FormTableGenerator<Exam>(examService.GetAll(), examService).Create(user);
                        break;
                    // promeni redosled
                    case "2":
                        new FormTableGenerator<Exam>(examService.GetAll(), examService).ShowTable();
                        break;
                    // radi
                    case "5":
                        new FormTableGenerator<Course>(courseService.GetAll(), courseService).Create(user);
                        break;
                    // promeni redosled
                    case "6":
                        new FormTableGenerator<Course>(courseService.GetAll(), courseService).ShowTable();
                        break;
                    case "9":
                        return;
                    default: break;
                }
            }catch (Exception ex){
                Console.WriteLine("Invalid entries. Try again.");
            }
        }
        private static void DirectorMenu(ITeacherService teacherService,IUserService userService, ICourseService courseService,IExamService examService)
        {
            Console.Write("" +
                "1) Create teachers\n" +
                "2) Read teachers\n" +
                "3) Update teachers\n" +
                "4) Delete teachers\n" +
                "5) Create courses - smart pick\n" +
                "6) Create exams - smart pick\n" +
                "7) Log out" +
                "Enter option >> ");
            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    new FormTableGenerator<Teacher>(teacherService.GetAll(),userService).Create(user);
                    break;
                case "2":
                    new FormTableGenerator<Teacher>(teacherService.GetAll(), teacherService).ShowTable();
                    break;
                // tehnicki radi, resiti problem creatorId = teacherId
                case "5":
                    object exam = new FormTableGenerator<Exam>(examService.GetAll(), examService).Create(user);
                    new FormTableGenerator<Teacher>(teacherService.GetAll(), teacherService).SmartPick(user, exam);
                    break;
                // radi
                case "6":
                    object item = new FormTableGenerator<Course>(courseService.GetAll(), courseService).Create(user);
                    new FormTableGenerator<Teacher>(teacherService.GetAll(), teacherService).SmartPick(user,item);
                    break;
                case "7":
                    return;
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
