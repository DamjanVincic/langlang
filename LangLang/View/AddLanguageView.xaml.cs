using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LangLang.Model;
using LangLang.ViewModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for AddLanguageView.xaml
    /// </summary>
    public partial class AddLanguageView : Window
    {
        public AddLanguageView(ICollectionView qualificationCollectionView)
        {
            DataContext = new AddLanguageViewModel(qualificationCollectionView);
            InitializeComponent();
        }
    }
}
