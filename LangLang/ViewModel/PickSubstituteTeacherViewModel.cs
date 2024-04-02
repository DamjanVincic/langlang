using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LangLang.Model;

namespace LangLang.ViewModel
{
    internal class PickSubstituteTeacherViewModel:ViewModelBase
    {
        private ObservableCollection<TeacherViewModel> displayedTeachers=new ObservableCollection<TeacherViewModel>();
        public ICollectionView TeachersCollectionView { get; }
        public ICommand SaveCommand { get; }
        public string Title { get; set; }
        public TeacherViewModel SelectedItem { get; set; }
        private Dictionary<Course, Teacher> substituteTeachers;
        private Course course;
        public PickSubstituteTeacherViewModel(List<Teacher> availableTeachers,Dictionary<Course,Teacher> substituteTeachers,Course course)
        {
            Title = "Select substitute teacher for course " + course.Language.ToString();
            this.substituteTeachers=substituteTeachers;
            this.course=course;
            foreach (Teacher teacher in availableTeachers)
            {
                displayedTeachers.Add(new TeacherViewModel(teacher));
            }
            TeachersCollectionView=CollectionViewSource.GetDefaultView(displayedTeachers); 
            SaveCommand = new RelayCommand(SaveSubstitute);
        }

        private void SaveSubstitute()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No teacher selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            substituteTeachers[course] = (Teacher)User.GetUserById(SelectedItem.Id);
            MessageBox.Show("Substitute teacher picked successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
