using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace EllipticBit.Controls.WPF.Extensions
{
	public class FontSize : MarkupExtension
	{
		[TypeConverter(typeof(FontSizeConverter))]
		public double Size { get; set; }

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return Size;
		}
	}
}