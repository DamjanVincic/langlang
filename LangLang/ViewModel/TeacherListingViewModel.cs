﻿using GalaSoft.MvvmLight;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using LangLang.View;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Teacher = LangLang.Model.Teacher;

namespace LangLang.ViewModel
{

    public class TeacherListingViewModel : ViewModelBase
    {
        private ObservableCollection<TeacherViewModel> teachers;
        public ICollectionView TeachersCollectionView { get; }
        public IEnumerable<String> LanguageNameValues => Language.LanguageNames;
        public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        private string _selectedLanguageName;
        private string _selectedLanguageLevel;
        private DateTime selectedDateCreated;
        public ICommand EditCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public TeacherViewModel SelectedItem { get; set; }
        public string SelectedLanguageName
        {
            get
            {
                return _selectedLanguageName;
            }
            set
            {
                _selectedLanguageName = value;
                TeachersCollectionView.Refresh();
            }
        }

        public string SelectedLanguageLevel
        {
            get
            {
                return _selectedLanguageLevel;
            }
            set
            {
                _selectedLanguageLevel = value;
                TeachersCollectionView.Refresh();
            }
        }
        public DateTime SelectedDateCreated
        {
            get
            {
                return selectedDateCreated;
            }
            set
            {
                selectedDateCreated = value;
                TeachersCollectionView.Refresh();
            }
        }
        public TeacherListingViewModel()
        {
            teachers=new ObservableCollection<TeacherViewModel>();
            TeachersCollectionView=CollectionViewSource.GetDefaultView(teachers);
            EditCommand = new RelayCommand(OpenEditWindow);
            AddCommand = new RelayCommand(OpenAddWindow);
            DeleteCommand = new RelayCommand(DeleteTeacher);

            // Language enga1 = new Language("English", LanguageLevel.A1);
            // Language enga2 = new Language("English", LanguageLevel.A2);
            // Language gera1 = new Language("German", LanguageLevel.A1);
            // Language gera2 = new Language("German", LanguageLevel.A2);
            // List<Language> peraLangs = new List<Language>
            // {
            //     enga1,
            //     gera1
            // };
            // Teacher t1 = new Teacher("Pera", "Peric", "mijat2004@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs);
            // Teacher t2 = new Teacher("Pera2", "Peric2", "kffjsdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs);
            // Teacher t3 = new Teacher("Pera3", "Peric3", "kfjsfdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs);
            // Teacher t4 = new Teacher("Pera4", "Peric4", "kfjsfdlk@gmkail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs);
            // Teacher t5 = new Teacher("Pera5", "Peric5", "kfjsfdlk@gjmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs);
            // Teacher t6 = new Teacher("Pera6", "Peric6", "kfjsfdlk@fgmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs);
            //
            // foreach (int teacherId in Teacher.TeacherIds)
            // {
            //     teachers.Add(new TeacherViewModel((Teacher)User.GetUserById(teacherId)));
            // }

            TeachersCollectionView.Filter=filterTeachers;
        }

        private bool filterTeachers(object obj)
        {
            if (obj is TeacherViewModel teacherViewModel)
            {
                return teacherViewModel.FilterLanguageName(SelectedLanguageName) && teacherViewModel.FilterLanguageLevel(SelectedLanguageLevel) && teacherViewModel.FilterDateCreated(SelectedDateCreated);
            }
            else
            {
                return false;
            }
        }

        private void OpenEditWindow()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No teacher selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newWindow = new EditTeacherView((Teacher)User.GetUserById(SelectedItem.Id), TeachersCollectionView);

            newWindow.Show();

        }

        private void OpenAddWindow()
        {
            var newWindow = new AddTeacherView(TeachersCollectionView,teachers);

            newWindow.Show();

        }

        private void DeleteTeacher()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("No teacher selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Teacher teacher = (Teacher)User.GetUserById(SelectedItem.Id);
            List<Course> ActiveCourses=new List<Course>();
            List<Course> CoursesCreatedByDirector = new List<Course>();
            List<Course> CoursesToBeDeleted=new List<Course>();

            foreach(int courseId in teacher.CourseIds)
            {
                Course course = Course.GetById(courseId);
                if (course.AreApplicationsClosed)
                {
                    ActiveCourses.Add(course);
                }
                else
                {
                    if (course.CreatorId != teacher.Id)
                    {
                        CoursesCreatedByDirector.Add(course);
                    }
                    else
                    {
                        CoursesToBeDeleted.Add(course);
                    }
                }
            }

            Dictionary<Course, Teacher> substituteTeachers=new Dictionary<Course, Teacher>();

            //ask for substitute teachers
            foreach (Course course in ActiveCourses)
            {
                List<Teacher> availableTeachers = new List<Teacher>();
                foreach(Teacher teacher_ in User.GetTeachers())
                {
                    if (Schedule.CanAddScheduleItem(course.StartDate, course.Duration, course.Held, teacher_.Id,
                            course.ScheduledTime, true, true))
                    {
                        availableTeachers.Add(teacher_);
                    }
                }
                //TODO: If there are no available teachers, return
                var newWindow = new PickSubstituteTeacherView(availableTeachers, substituteTeachers, course);

                newWindow.ShowDialog();
            }

            foreach (Course course in substituteTeachers.Keys)
            {
                course.TeacherId = substituteTeachers[course].Id;
            }

            foreach (int examId in teacher.ExamIds)
            {
                //shouldn't have id parameter
                Exam.GetById(examId).Delete();
            }

            foreach (Course course in CoursesCreatedByDirector)
            {
                course.CreatorId = -1;
            }

            foreach (Course course in CoursesToBeDeleted)
            {
                //delete course TBD
            }

            teachers.Remove(SelectedItem);
            //delete teacher object TBD
            TeachersCollectionView.Refresh();
        }


        public IEnumerable<TeacherViewModel> Teachers => teachers;
    }

}
