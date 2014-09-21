using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EllipticBit.Controls.WPF
{
	[ContentProperty("Items")]
	public class NestedTabControl : TabControl
	{
		public new ObservableCollection<NestedTabItem> Items { get { return (ObservableCollection<NestedTabItem>)GetValue(ItemsProperty); } set { SetValue(ItemsProperty, value); } }
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<NestedTabItem>), typeof(NestedTabControl));

		public NestedTabControl()
		{
			Items = new ObservableCollection<NestedTabItem>();
		}
	}

	[ContentProperty("Items")]
	public class NestedTabItem : TabItem
	{
		public ObservableCollection<NestedTabSubItem> Items { get { return (ObservableCollection<NestedTabSubItem>)GetValue(ItemsProperty); } set { SetValue(ItemsProperty, value); } }
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<NestedTabSubItem>), typeof(NestedTabItem));

		public NestedTabItem()
		{
			Items = new ObservableCollection<NestedTabSubItem>();
		}
	}

	public class NestedTabSubItem : TabItem
	{

	}
}
