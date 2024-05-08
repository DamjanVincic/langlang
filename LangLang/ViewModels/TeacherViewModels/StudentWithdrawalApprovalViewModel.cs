﻿using GalaSoft.MvvmLight;
using LangLang.Models;
using LangLang.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using LangLang.Repositories;
using LangLang.ViewModels.StudentViewModels;

namespace LangLang.ViewModels.TeacherViewModels
{
    class StudentWithdrawalApprovalViewModel : ViewModelBase
    {
        private readonly ICourseService _courseService = new CourseService();
        private readonly IStudentService _studentService = new StudentService();
        private readonly ICourseRepository _courseRepository = new CourseFileRepository();
        private readonly IUserRepository _userRepository = new UserFileRepository();
        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");
        private readonly int _courseId;
        public StudentWithdrawalApprovalViewModel(int courseId)
        {
            _courseId = courseId;
            StudentWithdrawals = new ObservableCollection<SingleStudentViewModel>();
            RefreshStudentWIthdrawalList();
            AcceptWithdrawalExplanationCommand= new RelayCommand(AcceptWithdrawalExplanation);
            RejectWithdrawalExplanationCommand = new RelayCommand(RejectWithdrawalExplanation);
        }

        public ObservableCollection<SingleStudentViewModel> StudentWithdrawals { get; set; }
        public SingleStudentViewModel? SelectedItem { get; set; }
        public ICommand AcceptWithdrawalExplanationCommand { get; set; }
        public ICommand RejectWithdrawalExplanationCommand { get; set; }

        private void AcceptWithdrawalExplanation()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No explanation selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string message = $"Are you sure you want to accept that withdrawal explanation?";
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(message, "Withdrawal explanation confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    Student student = _userRepository.GetById(SelectedItem.Id) as Student ??
                              throw new InvalidInputException("User doesn't exist.");
                    student.DropActiveCourse();
                    Course course = _courseRepository.GetById(_courseId) as Course ??
                        throw new InvalidInputException("Course doesn't exist.");
                    course.RemoveStudent(student.Id);

                    _userRepository.Update(student);
                    _courseRepository.Update(course);
                    MessageBox.Show("Successfully accepted withdrawal explanation.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshStudentWIthdrawalList();

                }
                catch (InvalidInputException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void RejectWithdrawalExplanation()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No explanation selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string message = $"Are you sure you want to reject that withdrawal explanation?";
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(message, "Withdrawal explanation confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    _studentService.AddPenaltyPoint(SelectedItem.Id, PenaltyPointReason.DroppingOutDenied, _courseId, _teacher.Id, DateOnly.FromDateTime(DateTime.Now));
                    Student student = _userRepository.GetById(SelectedItem.Id) as Student ??
                          throw new InvalidInputException("Student doesn't exist.");

                    student.DropActiveCourse();
                    Course course = _courseRepository.GetById(_courseId) as Course ??
                        throw new InvalidInputException("Course doesn't exist.");
                    course.RemoveStudent(student.Id);

                    _userRepository.Update(student);
                    _courseRepository.Update(course);
                    MessageBox.Show("Successfully rejected withdrawal explanation.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshStudentWIthdrawalList();

                }
                catch (InvalidInputException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RefreshStudentWIthdrawalList()
        {
            Course course = _courseRepository.GetById(_courseId) as Course ??
                throw new InvalidInputException("Course doesn't exist.");
            StudentWithdrawals.Clear();
            foreach (int studentId in course.DropOutRequests.Keys)
            {
                Student student = _userRepository.GetById(studentId) as Student ??
                  throw new InvalidInputException("Student doesn't exist.");

                StudentWithdrawals.Add(new SingleStudentViewModel(student));
            }
        }
    }
}
