﻿using System;
using System.Collections.Generic;
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
using LangLang.ViewModels.ExamViewModels;

namespace LangLang.Views.TeacherViews
{
    /// <summary>
    /// Interaction logic for CurrentExamView.xaml
    /// </summary>
    public partial class CurrentExamView : Window
    {
        public CurrentExamView()
        {
            InitializeComponent();
            DataContext = new CurrentExamViewModel(this);
        }
    }
}