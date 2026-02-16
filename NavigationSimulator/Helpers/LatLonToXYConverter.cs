using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace NavigationSimulator
{
    public class LatLonToXYConverter : MarkupExtension, IValueConverter
    {
        public bool IsLon { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val && parameter is string paramStr)
            {
                // Format: "Offset|Center|Scale"
                var parts = paramStr.Split('|');
                if (parts.Length == 3)
                {
                    double offset = double.Parse(parts[0], CultureInfo.InvariantCulture);
                    double center = double.Parse(parts[1], CultureInfo.InvariantCulture);
                    double scale = double.Parse(parts[2], CultureInfo.InvariantCulture);

                    if (IsLon)
                    {
                        return offset + (val - center) * scale;
                    }
                    else
                    {
                        // Y axis is inverted in WPF
                        return offset - (val - center) * scale;
                    }
                }
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
