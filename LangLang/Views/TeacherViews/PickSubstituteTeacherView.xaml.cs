using System.Collections.Generic;
using System.Windows;
using LangLang.Models;
using LangLang.ViewModels.TeacherViewModels;

namespace LangLang.Views.TeacherViews
{
    /// <summary>
    /// Interaction logic for PickSubstituteTeacherView.xaml
    /// </summary>
    public partial class PickSubstituteTeacherView : Window
    {
        public PickSubstituteTeacherView(List<Teacher> availableTeachers, ref int substituteTeacherId, Course course)
        {
            DataContext = new PickSubstituteTeacherViewModel(availableTeachers, ref substituteTeacherId, course);
            InitializeComponent();
        }
    }
}
