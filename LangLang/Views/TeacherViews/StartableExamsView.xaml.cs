<<<<<<<< HEAD:LangLang/Views/TeacherViews/AppliedExamView.xaml.cs
﻿using LangLang.ViewModels;
using System;
========
﻿using System;
>>>>>>>> develop:LangLang/Views/TeacherViews/StartableExamsView.xaml.cs
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
using LangLang.ViewModels.TeacherViewModels;

<<<<<<<< HEAD:LangLang/Views/TeacherViews/AppliedExamView.xaml.cs
namespace LangLang.Views
========
namespace LangLang.Views.TeacherViews
>>>>>>>> develop:LangLang/Views/TeacherViews/StartableExamsView.xaml.cs
{
    /// <summary>
    /// Interaction logic for StartableExamsView.xaml
    /// </summary>
    public partial class StartableExamsView : Window
    {
        public StartableExamsView()
        {
            InitializeComponent();
            DataContext = new StartableExamsViewModel();
        }
    }
}
