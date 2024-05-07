using GalaSoft.MvvmLight;
using LangLang.Services;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using LangLang.Models;
using LangLang.ViewModels.StudentViewModels;

namespace LangLang.ViewModels.CourseViewModels
{
    class StartCourseViewModel : ViewModelBase
    {
        private readonly ITeacherService _teacherService = new TeacherService();
        
        private readonly int _courseId;
        private readonly ICourseService _courseService = new CourseService();
        private readonly Window _startCourseWindow;
        public StartCourseViewModel(int courseId, Window startCourseWindow)
        {
            _courseId = courseId;
            _startCourseWindow = startCourseWindow;
            Students = new ObservableCollection<SingleStudentViewModel>(_courseService.GetStudents(_courseId)
                .Select(student => new SingleStudentViewModel(student)));
            ConfirmCommand = new RelayCommand(Confirm);
            RejectApplicationCommand = new RelayCommand(RejectApplication);

        }

        public ObservableCollection<SingleStudentViewModel> Students { get; set; }
        public SingleStudentViewModel? SelectedStudent { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand? RejectApplicationCommand { get; }

        private void Confirm()
        {
            _courseService.ConfirmCourse(_courseId);
            MessageBox.Show("Course started successfully.", "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
            _startCourseWindow.Close();

        }
        
        private void RejectApplication()
        {
            try
            {
                _teacherService.RejectStudentApplication(_courseId, SelectedStudent!.Id);
                MessageBox.Show("Student application rejected.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (InvalidInputException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
