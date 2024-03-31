using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.ViewModel
{
    public class ExamListingViewModel : ViewModelBase
    {
        ObservableCollection<ExamViewModel> exams;

        public ExamListingViewModel()
        {
            // set it as item source, that will be binding
            // every time we add or remove list view will update
            //_exams = new ObservableCollection<ExamViewModel>();

            // Definirajte kolekciju i dodajte tri Language objekta
            List<Language> languages = new List<Language>
            {
                new Language("English", LanguageLevel.B2),
                new Language("Math", LanguageLevel.A1),
                new Language("History", LanguageLevel.C1)
            };
            // Definirajte novu listu Exam objekata
            exams = new ObservableCollection<ExamViewModel>
            {
                new ExamViewModel(new Exam(1, languages[0], 20, new DateOnly(2024, 4, 15), new List<int> { 101, 102, 103 })),
                new ExamViewModel(new Exam(2, languages[0], 20, new DateOnly(2024, 4, 15), new List<int> { 101, 102, 103 })),
                new ExamViewModel(new Exam(3, languages[0], 20, new DateOnly(2024, 4, 15), new List<int> { 101, 102, 103 })),
            };
        }
        public IEnumerable<ExamViewModel> Exams => exams;

    }

}
