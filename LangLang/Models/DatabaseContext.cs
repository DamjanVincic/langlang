using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;

namespace LangLang.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    //public DbSet<CourseGrade> CourseGrades { get; set; }
    //public DbSet<Exam> Exams { get; set; }
    //public DbSet<ExamGrade> ExamGrades { get; set; }
    //public DbSet<Language> Languages { get; set; }
    //public DbSet<Message> Messages { get; set; }
    //public DbSet<PenaltyPoint> PenaltyPoints { get; set; }
    //public DbSet<ScheduleItem> ScheduleItems { get; set; }
    //public DbSet<User> Users { get; set; }
    // public DbSet<LanguageLevel> LanguageLevels { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Host=localhost;Username=user;Password=MnogoJakaSifra123;Persist Security Info=True;Database=langlang");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
            dateTime => DateOnly.FromDateTime(dateTime)
        );

        var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
            timeOnly => timeOnly.ToTimeSpan(),
            timeSpan => TimeOnly.FromTimeSpan(timeSpan)
        );

        modelBuilder.Entity<Course>()
            .Property(e => e.Date)
            .HasConversion(dateOnlyConverter);

        modelBuilder.Entity<Course>()
            .Property(e => e.StartDate)
            .HasConversion(dateOnlyConverter);

        modelBuilder.Entity<Course>()
            .Property(e => e.ScheduledTime)
            .HasConversion(timeOnlyConverter);

        modelBuilder.Entity<List<Weekday>>().HasNoKey();

        base.OnModelCreating(modelBuilder);
    }
}