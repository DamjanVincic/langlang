using System.Windows;
using LangLang.ViewModel;

namespace LangLang.Views;

public partial class StudentCourseView : Window
{
    public StudentCourseView()
    {
        InitializeComponent();
        DataContext = new StudentCourseViewModel();
    }
}