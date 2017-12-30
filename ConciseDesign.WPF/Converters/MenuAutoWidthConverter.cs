using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace ConciseDesign.WPF.Converters
{
    class MenuAutoWidthConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameters, System.Globalization.CultureInfo info)
        {
            var ctls = value as ItemsControl;
            return ctls.Width / ctls.Items.Count - 4;
        }
        public object ConvertBack(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
