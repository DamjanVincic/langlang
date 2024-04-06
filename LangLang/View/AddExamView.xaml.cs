﻿using LangLang.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LangLang.Model;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for ExamView.xaml
    /// </summary>
    public partial class AddExamView : Window
    {
        public AddExamView(ObservableCollection<ExamViewModel> exams, ICollectionView examCollectionView, Exam exam = null, Teacher teacher = null)
        {
            DataContext = new AddExamViewModel(exams, examCollectionView, exam, teacher, this);
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
