using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LangLang.ViewModels.CourseViewModels;

namespace LangLang.Views.CourseViews
{
    /// <summary>
    /// Interaction logic for CourseListingView.xaml
    /// </summary>
    public partial class CourseListingView : Window
    {
        public CourseListingView()
        {
            InitializeComponent();
            DataContext = new CourseListingViewModel();
        }
        public class MaxStudentsVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string? format = value as string;
                if (format.Equals("in-person"))
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
