using System.Collections.Generic;
using System.Windows;
using LangLang.Models;
using LangLang.ViewModels;

namespace LangLang.Views.TeacherViews
{
    /// <summary>
    /// Interaction logic for PickSubstituteTeacherView.xaml
    /// </summary>
    public partial class PickSubstituteTeacherView : Window
    {
        public PickSubstituteTeacherView(List<Teacher> availableTeachers, Dictionary<Course, Teacher> substituteTeachers, Course course)
        {
            DataContext = new PickSubstituteTeacherViewModel(availableTeachers, substituteTeachers, course);
            InitializeComponent();
        }
    }
}
