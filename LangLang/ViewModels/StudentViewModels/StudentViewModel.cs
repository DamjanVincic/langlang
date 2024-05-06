﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.StudentViews;

namespace LangLang.ViewModels.StudentViewModels;

public class StudentViewModel : ViewModelBase
{
    private readonly IUserService _userService = new UserService();

    private readonly Student _student = UserService.LoggedInUser as Student ?? throw new InvalidInputException("No one is logged in.");
    private readonly CourseService _courseService = new CourseService();
    private readonly Course? _course;

    private readonly Window _studentViewWindow;

    public StudentViewModel(Window studentViewWindow)
    {
        _studentViewWindow = studentViewWindow;

        ViewCoursesCommand = new RelayCommand(ViewCourses);
        ViewAppliedCoursesCommand = new RelayCommand(ViewAppliedCourses);
        ViewExamsCommand = new RelayCommand(ViewExams);
        ViewAppliedExamsCommand = new RelayCommand(ViewAppliedExams);
        EditAccountCommand = new RelayCommand(EditAccount);
        DeleteAccountCommand = new RelayCommand(DeleteAccount);
        LogOutCommand = new RelayCommand(LogOut);

        if (_student.ActiveCourseId.HasValue)
        {
            _course = _courseService.GetById(_student.ActiveCourseId.Value);
        }
        else
        {
            _course = null;
        }
    }

    public int NumberOfPenaltyPoints
    {
        get => _student.PenaltyPoints;
        set
        {
            _student.PenaltyPoints = value;
        }
    }
    public string LanguageName
    {
        get => _course?.Language?.Name ?? "No language";
        set
        {
            if (_course != null && _course.Language != null)
            {
                _course.Language.Name = value;
            }
        }
    }

    public string LanguageLevel
    {
        get => _course?.Language?.Level.ToString() ?? "No level";
        set
        {
            if (_course != null && Enum.TryParse<LanguageLevel>(value, out LanguageLevel level))
            {
                _course.Language.Level = level;
            }
        }
    }

    public string DaysHeld
    {
        get => _course != null ? string.Join(",", _course.Held.Select(d => d.ToString())) : "Unavailable";
        set
        {
            if (_course != null)
            {
                _course.Held = value.Split(',')
                                     .Select(s => (Weekday)Enum.Parse(typeof(Weekday), s))
                                     .ToList();
            }
        }
    }

    public string Time
    {
        get => _course?.ScheduledTime.ToString("HH:mm:ss") ?? "No scheduled time";
        set
        {
            if (_course != null && TimeOnly.TryParseExact(value, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly time))
            {
                _course.ScheduledTime = time;
                RaisePropertyChanged();
            }
        }
    }


    public ObservableCollection<Course> AvailableCourses { get; set; }
    public ObservableCollection<Exam> AvailableExams { get; set; }

    public string FullName => $"{_student.FirstName} {_student.LastName}";

    public ICommand ViewCoursesCommand { get; }
    public ICommand ViewAppliedCoursesCommand { get; }
    public ICommand ViewExamsCommand { get; }
    public ICommand ViewAppliedExamsCommand { get; }

    public ICommand EditAccountCommand { get; }
    public ICommand DeleteAccountCommand { get; }
    public ICommand LogOutCommand { get; }

    private static void ViewCourses()
    {
        new StudentCourseView().Show();
    }

    private static void ViewAppliedCourses()
    {
        new StudentCourseView(true).Show();
    }

    private static void ViewExams()
    {
        new StudentExamView().Show();
    }
    private static void ViewAppliedExams()
    {
        new AppliedExamView().Show();
    }

    private void EditAccount()
    {
        // TO DO: in teacher, after exam is over remove it from appliedExams, ensure that only future exams are in the list or none at all
        if(_student.AppliedExams.Count != 0)
        {
            MessageBox.Show("You cannot change your data while you have registered exams.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        new StudentEditView().ShowDialog();
    }

    private void DeleteAccount()
    {
        if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

        _userService.Delete(_student.Id);
        MessageBox.Show("Account deleted successfully");
        
        new MainWindow().Show();
        _studentViewWindow.Close();
    }

    private void LogOut()
    {
        _userService.Logout();
        new MainWindow().Show();
        _studentViewWindow.Close();
    }
}