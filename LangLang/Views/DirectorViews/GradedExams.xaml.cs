using System.Windows;
using LangLang.ViewModels.DirectorViewModels;

namespace LangLang.Views.DirectorViews
{
    /// <summary>
    /// Interaction logic for GradedExams.xaml
    /// </summary>
    public partial class GradedExams : Window
    {
        public GradedExams()
        {
            InitializeComponent();
            DataContext = new GradedExamsViewModel();
        }
    }
}
