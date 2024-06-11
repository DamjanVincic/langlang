using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LangLang.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            DotNetEnv.Env.Load("../.env"); // Works if the current directory is the LangLang project
            string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? throw new InvalidInputException("Connection string not found in .env file.");
            optionsBuilder.UseNpgsql(connectionString);
        }
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
        
        modelBuilder.Entity<Course>()
            .Property(c => c.Held)
            .HasConversion(
                v => JsonConvert.SerializeObject(v), // Convert list to JSON string
                v => JsonConvert.DeserializeObject<List<Weekday>>(v) // Convert JSON string to list
            );
    }
}