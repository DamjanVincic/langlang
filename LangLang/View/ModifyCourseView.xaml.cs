using LangLang.Model;
using LangLang.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for AddCourseView.xaml
    /// </summary>
    public partial class ModifyCourseView : Window
    {
        private List<string> selectedWeekdays = new List<string>();

        public ModifyCourseView(ObservableCollection<CourseViewModel> _courses, ICollectionView courseCollectionView,
            int teacherId, Course course = null)
        {
            InitializeComponent();
            DataContext = new ModifyCourseViewModel(_courses, courseCollectionView, teacherId, course);
        }
    }
}