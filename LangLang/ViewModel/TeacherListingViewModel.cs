using GalaSoft.MvvmLight;
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

namespace LangLang.ViewModel
{

    public class TeacherListingViewModel : ViewModelBase
    {
        private ObservableCollection<TeacherViewModel> teachers;
        public ICollectionView TeachersCollectionView { get; }
        public IEnumerable<String> LanguageNameValues => Language.LanguageNames;
        public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        private string selectedLanguageName;
        private string selectedLanguageLevel;
        private DateTime selectedDateCreated;
        public string SelectedLanguageName
        {
            get
            {
                return selectedLanguageName;
            }
            set
            {
                selectedLanguageName = value;
                TeachersCollectionView.Refresh();
            }
        }

        public string SelectedLanguageLevel
        {
            get
            {
                return selectedLanguageLevel;
            }
            set
            {
                selectedLanguageLevel = value;
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

            Language enga1 = new Language("English", LanguageLevel.A1);
            Language enga2 = new Language("English", LanguageLevel.A2);
            Language gera1 = new Language("German", LanguageLevel.A1);
            Language gera2 = new Language("German", LanguageLevel.A2);
            List<Language> peraLangs = new List<Language>
            {
                enga1,
                gera1
            };
            Teacher t1 = new Teacher("Pera", "Peric", "mijat2004@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t2 = new Teacher("Pera2", "Peric2", "kffjsdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t3 = new Teacher("Pera3", "Peric3", "kfjsfdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t4 = new Teacher("Pera4", "Peric4", "kfjsfdlk@gmkail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t5 = new Teacher("Pera5", "Peric5", "kfjsfdlk@gjmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t6 = new Teacher("Pera6", "Peric6", "kfjsfdlk@fgmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });

            foreach (int teacherId in Teacher.TeacherIds)
            {
                teachers.Add(new TeacherViewModel((Teacher)User.GetById(teacherId)));
            }

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

        public IEnumerable<TeacherViewModel> Teachers => teachers;
    }

}
