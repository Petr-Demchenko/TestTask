using System.Globalization;
using System.Windows.Data;

namespace MedTech.Helpers
{
    internal class DateTimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if (values[0] is DateTime dateTime && values[1] is bool isChecked)
                {
                    return (isChecked ? dateTime : dateTime.ToLocalTime()).ToString();
                }
            }

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
