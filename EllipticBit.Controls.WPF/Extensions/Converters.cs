using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Xml;

namespace EllipticBit.Controls.WPF.Extensions
{
	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class BoolVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var v = (bool)value;
			var param = System.Convert.ToBoolean(parameter);
			return v == false ? param ? Visibility.Hidden : Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(bool), typeof(Visibility))]
	public class NotBoolVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var v = (bool)value;
			var param = System.Convert.ToBoolean(parameter);
			return v ? param ? Visibility.Hidden : Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(bool), typeof(bool))]
	public class NotBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var v = (bool)value;
			return !v;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(object), typeof(bool))]
	public class NullBoolValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool result = value == null;
			if (parameter != null)
				return !result;
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}

	[ValueConversion(typeof(object), typeof(Visibility))]
	public class NullVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var param = System.Convert.ToBoolean(parameter);
			return value == null ? param ? Visibility.Hidden : Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[ValueConversion(typeof(string), typeof(string))]
	public class ToUppercaseValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return "";
			string v = System.Convert.ToString(value);
			return v.ToUpper();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}

	[ValueConversion(typeof(long), typeof(string))]
	public class NullLongConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? "" : System.Convert.ToString(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var lt = (string)value;
			if (lt == "") return null;
			try { return System.Convert.ToInt64(lt); }
			catch { return null; }
		}
	}

	[ValueConversion(typeof(long), typeof(string))]
	public class LongConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return System.Convert.ToString(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var lt = (string)value;
			return lt == "" ? 0 : System.Convert.ToInt64(lt);
		}
	}

	[ValueConversion(typeof(long), typeof(string))]
	public class IntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return System.Convert.ToString(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var lt = (string)value;
			return lt == "" ? 0 : System.Convert.ToInt64(lt);
		}
	}

	public class UriToBitmapImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var image = new BitmapImage();
			if (value != null)
			{
				try
				{
					image.BeginInit();
					image.CacheOption = BitmapCacheOption.OnLoad;
					image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
					image.UriSource = new Uri((string)value, UriKind.Absolute);
					image.EndInit();
				}
				catch
				{
					image = null;
				}
			}

			return image;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}

	[ValueConversion(typeof(string), typeof(DateTime))]
	public class XmlDateConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var t = value as string;
			return t != null ? System.Convert.ToDateTime(t) : DateTime.Now;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return XmlConvert.ToString((DateTime)value, XmlDateTimeSerializationMode.Utc);
		}

		#endregion
	}

	[ValueConversion(typeof(string), typeof(string))]
	public class StripHTMLConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return "";
			var html = new Regex("<[^>]+>");
			string v = System.Convert.ToString(value);
			string ret = "";
			foreach (string s in v.Split('\n'))
				ret += html.Replace(s, "");
			return ret.Replace("\t", "").Replace("&nbsp;", " ");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}