using System;
using System.Collections.Generic;
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
using LangLang.Model;
using LangLang.ViewModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for EditTeacherView.xaml
    /// </summary>
    public partial class EditTeacherView : Window
    {
        public EditTeacherView(Teacher teacher, ICollectionView teachersCollectionView)
        {
            DataContext = new EditTeacherViewModel(teacher, teachersCollectionView, this);
            InitializeComponent();
        }
    }
}
