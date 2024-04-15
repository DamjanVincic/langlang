using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views;

public partial class StudentCourseView : Window
{
    public StudentCourseView()
    {
        InitializeComponent();
        DataContext = new StudentCourseViewModel();
    }
}