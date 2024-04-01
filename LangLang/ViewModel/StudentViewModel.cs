using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;

namespace LangLang.ViewModel;

public class StudentViewModel : ViewModelBase
{
    public Student Student { get; set; }
    public ObservableCollection<Course> AvailableCourses { get; set; }
    public ObservableCollection<Exam> AvailableExams { get; set; }
    
    public string FullName => $"{Student.FirstName} {Student.LastName}";
    
    public ICommand ApplyForCourseCommand { get; }
    public ICommand ApplyForExamCommand { get; }
    
    public StudentViewModel(Student student)
    {
        Student = student;
        // AvailableCourses = new ObservableCollection<Course>(Course.GetAvailableCourses());
        // AvailableExams = new ObservableCollection<Exam>(Exam.GetAvailableExams());
        //
        // ApplyForCourseCommand = new RelayCommand<Course>(ApplyForCourse);
        // ApplyForExamCommand = new RelayCommand<Exam>(ApplyForExam);
    }
}