using System.Windows;
using LangLang.ViewModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for AddLanguageView.xaml
    /// </summary>
    public partial class AddLanguageView : Window
    {
        public AddLanguageView()
        {
            DataContext = new AddLanguageViewModel(this);
            InitializeComponent();
        }
    }
}
