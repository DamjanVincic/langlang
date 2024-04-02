using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;

namespace LangLang.ViewModel;

public class StudentViewModel : ViewModelBase
{
    private Student _student;
    public ObservableCollection<Course> AvailableCourses { get; set; }
    public ObservableCollection<Exam> AvailableExams { get; set; }
    
    public string FullName => $"{_student.FirstName} {_student.LastName}";
    
    public ICommand EditAccountCommand { get; }
    public ICommand ViewCoursesCommand { get; }
    public ICommand ViewExamsCommand { get; }
    public ICommand ApplyForCourseCommand { get; }
    public ICommand ApplyForExamCommand { get; }
    
    public StudentViewModel(Student student)
    {
        _student = student;
        // AvailableCourses = new ObservableCollection<Course>(Course.GetAvailableCourses());
        // AvailableExams = new ObservableCollection<Exam>(Exam.GetAvailableExams());
        //
        // ApplyForCourseCommand = new RelayCommand<Course>(ApplyForCourse);
        // ApplyForExamCommand = new RelayCommand<Exam>(ApplyForExam);
        EditAccountCommand = new RelayCommand(EditAccount);
        ViewCoursesCommand = new RelayCommand(ViewCourses);
        ViewExamsCommand = new RelayCommand(ViewExams);
    }
    
    private void EditAccount()
    {
        new StudentEditView(_student).Show();
    }
    
    private void ViewCourses()
    {
        new StudentCourseView().Show();
    }
    
    private void ViewExams()
    {
        new StudentExamView().Show();
    }
}