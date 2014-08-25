using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EllipticBit.Controls.WPF
{
	public class HorizontalNav : TabControl
	{
		public Button Options { get { return (Button)GetValue(OptionsProperty); } set { SetValue(OptionsProperty, value); } }
		public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(Button), typeof(HorizontalNav), new PropertyMetadata(new SubtleButton()));

	}
}