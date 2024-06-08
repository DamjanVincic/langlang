using Microsoft.EntityFrameworkCore;

namespace LangLang.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseGrade> CourseGrades { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamGrade> ExamGrades { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<PenaltyPoint> PenaltyPoints { get; set; }
    public DbSet<ScheduleItem> ScheduleItems { get; set; }
    public DbSet<User> Users { get; set; }
    // public DbSet<LanguageLevel> LanguageLevels { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Host=localhost;Username=user;Password=MnogoJakaSifra123;Persist Security Info=True;Database=langlang");
    }
}