using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EllipticBit.Controls.WPF
{
	public class MultiComboBox : ComboBox
	{
		public MultiComboBox()
		{
			SelectedItems = new ObservableCollection<object>();
		}

		public ObservableCollection<object> SelectedItems { get { return (ObservableCollection<object>)GetValue(SelectedItemsProperty); } set { SetValue(SelectedItemsProperty, value); } }
		public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(MultiComboBox), new PropertyMetadata(null));

		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);

			foreach (var t in e.AddedItems)
				SelectedItems.Add(t);
		}
	}

	public class GroupComboBox : ComboBox
	{
		public new ObservableCollection<ComboBoxItem> Items { get { return (ObservableCollection<ComboBoxItem>)GetValue(ItemsProperty); } set { SetValue(ItemsProperty, value); } }
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<ComboBoxItem>), typeof(GroupComboBox), new PropertyMetadata(null));

		public GroupComboBox()
		{
			Items = new ObservableCollection<ComboBoxItem>();
		}
	}

	public class ComboBoxItemGroup : ComboBoxItem
	{
		public object Header { get { return (object)GetValue(HeaderProperty); } set { SetValue(HeaderProperty, value); } }
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(ComboBoxItemGroup), new PropertyMetadata(null));

		public ObservableCollection<ComboBoxItem> Items { get { return (ObservableCollection<ComboBoxItem>)GetValue(ItemsProperty); } set { SetValue(ItemsProperty, value); } }
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<ComboBoxItem>), typeof(ComboBoxItemGroup), new PropertyMetadata(null));

		public ComboBoxItemGroup()
		{
			Items = new ObservableCollection<ComboBoxItem>();

		}

		protected override void OnSelected(RoutedEventArgs e)
		{
		}
	}
}