using System.Windows;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class StudentCourseView : Window
{
    public StudentCourseView()
    {
        InitializeComponent();
        DataContext = new StudentCourseViewModel();
    }
}