using System.Linq;
using System.Windows;
using LangLang.Models;
using LangLang.Repositories;
using LangLang.Services;
using LangLang.Services.ReportServices;
using LangLang.ViewModels.CourseViewModels;
using LangLang.ViewModels.DirectorViewModels;
using LangLang.ViewModels.ExamViewModels;
using LangLang.ViewModels.StudentViewModels;
using LangLang.ViewModels.TeacherViewModels;
using Microsoft.Extensions.DependencyInjection;
using ServiceProvider = LangLang.Models.ServiceProvider;

namespace LangLang
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider.Instance = services.BuildServiceProvider();
            
            Director director = new Director("Nadja", "Zoric", "nadjazoric@gmail.com", "PatrikZvezdasti011", Gender.Female, "1234567890123");
            
            IUserRepository userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            if (userRepository.GetAll().All(user => user.Email != director.Email))
                userRepository.Add(director);
            
            Exit += App_Exit;
        }
        
        private void ConfigureServices(IServiceCollection services)
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

            services.AddTransient<MainWindow>();
            services.AddTransient<ActiveCoursesViewModel>();
            services.AddTransient<AddCourseViewModel>();
            services.AddTransient<CourseListingViewModel>();
            services.AddTransient<CoursesWithStudentWithdrawalsViewModel>();
            services.AddTransient<StartableCoursesViewModel>();
            services.AddTransient<GradedExamsViewModel>();
            services.AddTransient<ExamListingViewModel>();
            services.AddTransient<AppliedExamListingViewModel>();
            services.AddTransient<StudentExamViewModel>();
            services.AddTransient<StartableExamsViewModel>();
            services.AddTransient<BestStudentsNotificationViewModel>();
            services.AddTransient<CourseListingDirectorViewModel>();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        
        private void App_Exit(object sender, ExitEventArgs e)
        {
            
        }
    }
}
