﻿// <auto-generated />
using System;
using System.Collections.Generic;
using LangLang.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LangLang.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LangLang.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("AreApplicationsClosed")
                        .HasColumnType("boolean");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<List<int>>("Held")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("boolean");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<int>("MaxStudents")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("ScheduledTime")
                        .HasColumnType("interval");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("StudentsNotified")
                        .HasColumnType("boolean");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("LangLang.Models.Exam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<bool>("DirectorGraded")
                        .HasColumnType("boolean");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<int>("MaxStudents")
                        .HasColumnType("integer");

                    b.Property<TimeOnly>("ScheduledTime")
                        .HasColumnType("time without time zone");

                    b.Property<List<int>>("StudentIds")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<bool>("TeacherGraded")
                        .HasColumnType("boolean");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("LangLang.Models.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("LangLang.Models.ScheduleItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Confirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LanguageId")
                        .HasColumnType("integer");

                    b.Property<int>("MaxStudents")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("ScheduledTime")
                        .HasColumnType("interval");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.ToTable("ScheduleItems");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ScheduleItem");
                });

            modelBuilder.Entity("LangLang.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("System.Collections.Generic.Dictionary<int, bool>", b =>
                {
                    b.ToTable("Dictionary<int, bool>");
                });

            modelBuilder.Entity("System.Collections.Generic.Dictionary<int, int>", b =>
                {
                    b.ToTable("Dictionary<int, int>");
                });

            modelBuilder.Entity("System.Collections.Generic.List<LangLang.Models.Weekday>", b =>
                {
                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.ToTable("List<Weekday>");
                });

            modelBuilder.Entity("LangLang.Models.Course", b =>
                {
                    b.HasBaseType("LangLang.Models.ScheduleItem");

                    b.Property<bool>("AreApplicationsClosed")
                        .HasColumnType("boolean");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("integer");

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<List<int>>("Held")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("StudentsNotified")
                        .HasColumnType("boolean");

                    b.HasDiscriminator().HasValue("Course");
                });

            modelBuilder.Entity("LangLang.Models.Director", b =>
                {
                    b.HasBaseType("LangLang.Models.User");

                    b.HasDiscriminator().HasValue("Director");
                });

            modelBuilder.Entity("LangLang.Models.Student", b =>
                {
                    b.HasBaseType("LangLang.Models.User");

                    b.Property<int?>("ActiveCourseId")
                        .HasColumnType("integer");

                    b.Property<List<int>>("AppliedExams")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("CourseGradeIds")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Education")
                        .HasColumnType("integer");

                    b.Property<string>("ExamGradeIds")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LanguagePassFail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PenaltyPoints")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("LangLang.Models.Teacher", b =>
                {
                    b.HasBaseType("LangLang.Models.User");

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("LangLang.Models.ScheduleItem", b =>
                {
                    b.HasOne("LangLang.Models.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");
                });

            modelBuilder.Entity("LangLang.Models.Exam", b =>
                {
                    b.HasOne("LangLang.Models.Language", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");
                });

            modelBuilder.Entity("LangLang.Models.Language", b =>
                {
                    b.HasOne("LangLang.Models.Teacher", null)
                        .WithMany("Qualifications")
                        .HasForeignKey("TeacherId");
                });

            modelBuilder.Entity("LangLang.Models.Teacher", b =>
                {
                    b.Navigation("Qualifications");
                });
#pragma warning restore 612, 618
        }
    }
}
