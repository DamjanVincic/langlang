using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LangLang.Model;
using LangLang.ViewModel;

namespace LangLang.View
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
