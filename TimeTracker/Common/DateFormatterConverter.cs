using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TimeTracker.Common
{
    public sealed class TimeTakenFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan timeTaken = (TimeSpan) value;
            string returnValue = timeTaken.Hours.ToString() + " hours " + timeTaken.Minutes.ToString() + " minutes " +
                                 timeTaken.Seconds.ToString() + " seconds";
            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
