using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LangLang.ViewModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for CourseView.xaml
    /// </summary>
    public partial class CourseView : Window
    {
        public CourseView()
        {
            InitializeComponent();
            DataContext = new CourseListingViewModel();
        }
        public class MaxStudentsVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string format = value as string;
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
