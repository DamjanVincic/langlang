﻿using System.Windows;
using LangLang.ViewModels;

namespace LangLang.Views;

public partial class StudentView : Window
{
    public StudentView()
    {
        InitializeComponent();
        DataContext = new StudentViewModel(this);
    }
}