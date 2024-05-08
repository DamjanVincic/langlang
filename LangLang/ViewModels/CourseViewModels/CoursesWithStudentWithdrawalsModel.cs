using GalaSoft.MvvmLight;
using LangLang.Models;
using LangLang.Services;
using LangLang.Views.CourseViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using LangLang.Views.TeacherViews;

namespace LangLang.ViewModels.CourseViewModels
{
    class CoursesWithStudentWithdrawalsModel : ViewModelBase
    {
        private readonly ICourseService _courseService = new CourseService();
        private readonly Teacher _teacher = UserService.LoggedInUser as Teacher ??
                                            throw new InvalidOperationException("No one is logged in.");
        private readonly ObservableCollection<CourseViewModel> _coursesWithWithdrawals;

        public CoursesWithStudentWithdrawalsModel()
        {
            _coursesWithWithdrawals = new ObservableCollection<CourseViewModel>(_courseService.GetCoursesWithWithdrawals(_teacher.Id).Select(course => new CourseViewModel(course)));
            CoursesWithWithdrawalsCollectionView = CollectionViewSource.GetDefaultView(_coursesWithWithdrawals);
            SeeWithdrawalsListCommand = new RelayCommand(SeeWithdrawalsList);
        }
        public ICollectionView CoursesWithWithdrawalsCollectionView { get; }

        public ObservableCollection<CourseViewModel> CoursesWithWithdrawals => _coursesWithWithdrawals;
        public CourseViewModel? SelectedItem { get; set; }
        public ICommand SeeWithdrawalsListCommand { get; set; }

        private void SeeWithdrawalsList()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No course selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newWindow = new StudentWithdrawalApprovalView(SelectedItem.Id);

            newWindow.ShowDialog();
            RefreshCoursesWithWithdrawals();
        }

        private void RefreshCoursesWithWithdrawals()
        {
            CoursesWithWithdrawals.Clear();
            _courseService.GetCoursesWithWithdrawals(_teacher.Id).ForEach(course => CoursesWithWithdrawals.Add(new CourseViewModel(course)));
            CoursesWithWithdrawalsCollectionView.Refresh();
        }
    }
}

