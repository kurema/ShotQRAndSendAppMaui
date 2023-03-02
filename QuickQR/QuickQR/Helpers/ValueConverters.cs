using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQR.Helpers.ValueConverters;

public class BoolToStringValueConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var p = parameter.ToString().Split(':');
		if (p.Length != 2) return null;
		if (value is bool b || bool.TryParse(value.ToString(), out b)) return b ? p[0] : p[1];
		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
