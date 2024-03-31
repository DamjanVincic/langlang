using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ViewModel
{

    public class TeacherListingViewModel:ViewModelBase
    {
        ObservableCollection<TeacherViewModel> teachers;
        public TeacherListingViewModel()
        {
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
            Teacher t2 = new Teacher("Pera2", "Peric2", "kfjsdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t3 = new Teacher("Pera3", "Peric3", "kfjsdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t4 = new Teacher("Pera4", "Peric4", "kfjsdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t5 = new Teacher("Pera5", "Peric5", "kfjsdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });
            Teacher t6 = new Teacher("Pera6", "Peric6", "kfjsdlk@gmail.com", "Lozinkaa2", Gender.Male, "0638662250", peraLangs, new List<int> { 1, 2, 3 });

            teachers = new ObservableCollection<TeacherViewModel> {
                new TeacherViewModel(t1),
                new TeacherViewModel(t2),
                new TeacherViewModel(t3),
                new TeacherViewModel(t4),
                new TeacherViewModel(t5),
                new TeacherViewModel(t6)
            };
        }

        public IEnumerable<TeacherViewModel> Teachers => teachers;
    }
        
}
