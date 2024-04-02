using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;
using LangLang.View;
using System.Windows;
using System.Windows.Input;

namespace LangLang.ViewModel
{
    class TeacherMenuViewModel : ViewModelBase
    {
        public TeacherMenuViewModel(Teacher teacher)
        {
            CourseCommand = new RelayCommand(Course);
            ExamCommand = new RelayCommand(Exam);
        }

        public ICommand CourseCommand { get; }
        public void Course()
        {
            var newWindow = new CourseView();
            newWindow.Show();
            Application.Current.MainWindow.Closed += (sender, e) =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Application.Current.MainWindow)
                    {
                        window.Close();
                    }
                }
            };
        }
        public ICommand ExamCommand { get; }
        public void Exam()
        {
            var newWindow = new ExamView();
            newWindow.Show();
            Application.Current.MainWindow.Closed += (sender, e) =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != Application.Current.MainWindow)
                    {
                        window.Close();
                    }
                }
            };
        }
    }
}