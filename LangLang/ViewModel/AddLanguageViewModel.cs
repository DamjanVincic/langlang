using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LangLang.Model;

namespace LangLang.ViewModel
{
    internal class AddLanguageViewModel:ViewModelBase
    {
        public string LanguageName { get; set; }
        public LanguageLevel SelectedLanguageLevel { get; set; }
        public IEnumerable<String> LanguageLevelValues => Enum.GetNames(typeof(LanguageLevel));
        private ICollectionView qualificationCollectionView;

        public ICommand AddLanguageCommand { get; }
        
        private Window _addLanguageWindow;

        public AddLanguageViewModel(ICollectionView qualificationCollectionView, Window addLanguageWindow)
        {
            _addLanguageWindow = addLanguageWindow;
            
            this.qualificationCollectionView = qualificationCollectionView;
            AddLanguageCommand = new RelayCommand(AddLanguage);
        }
        public void AddLanguage()
        {
            try
            {
                Language.MakeLanguage(LanguageName, SelectedLanguageLevel);
                qualificationCollectionView.Refresh();
                MessageBox.Show("Language added successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                _addLanguageWindow.Close();
            }
            catch (InvalidInputException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentNullException exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
