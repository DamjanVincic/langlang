using System.Windows;
using LangLang.ViewModels.StudentViewModels;

namespace LangLang.Views.StudentViews;

public partial class StudentCourseView : Window
{
    public StudentCourseView()
    {
        InitializeComponent();
        DataContext = new StudentCourseViewModel();
    }
}