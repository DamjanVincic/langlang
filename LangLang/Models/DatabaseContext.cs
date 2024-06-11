using Microsoft.EntityFrameworkCore;

namespace LangLang.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}