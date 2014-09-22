using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EllipticBit.Controls.WPF
{
	[ContentProperty("Items")]
	public class TabGroupControl : ItemsControl
	{
		public delegate void SelectedTabChangedEventHandler(object sender, RoutedEventArgs e);
		public static readonly RoutedEvent SelectedTabChangedEvent = EventManager.RegisterRoutedEvent("SelectedTabChanged", RoutingStrategy.Bubble, typeof(SelectedTabChangedEventHandler), typeof(TabGroupControl));

		public object SelectedContent { get { return (object)GetValue(SelectedContentProperty); } set { SetValue(SelectedContentProperty, value); } }
		public static readonly DependencyProperty SelectedContentProperty = DependencyProperty.Register("SelectedContent", typeof(object), typeof(TabGroupControl));

		public TabGroupItem SelectedTab { get { return (TabGroupItem)GetValue(SelectedTabProperty); } set { SetValue(SelectedTabProperty, value); } }
		public static readonly DependencyProperty SelectedTabProperty = DependencyProperty.Register("SelectedTab", typeof(TabGroupItem), typeof(TabGroupControl), new PropertyMetadata(SelectedTab_PropertyChanged));

		public new ObservableCollection<TabGroup> Items { get { return (ObservableCollection<TabGroup>)GetValue(ItemsProperty); } set { SetValue(ItemsProperty, value); } }
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<TabGroup>), typeof(TabGroupControl));

		public TabGroupControl()
		{
			Items = new ObservableCollection<TabGroup>();
			Items.CollectionChanged += Items_CollectionChanged;
		}

		public event SelectedTabChangedEventHandler SelectedTabChanged
		{
			add { AddHandler(SelectedTabChangedEvent, value); }
			remove { RemoveHandler(SelectedTabChangedEvent, value); }
		}

		void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count > 0)
			{
				foreach (var t in e.NewItems.OfType<TabGroup>())
					t.Parent = this;
			}

			if (e.OldItems == null || e.OldItems.Count <= 0) return;
			foreach (var t in e.OldItems.OfType<TabGroup>())
				t.Parent = null;
		}

		private static void SelectedTab_PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var tgc = obj as TabGroupControl;
			var tgi = e.NewValue as TabGroupItem;
			if (tgc == null) return;

			foreach (var y in (from x in tgc.Items from z in x.Items where !Equals(z, tgi) select z).Where(a => a != null))
				y.IsSelected = false;

			tgc.RaiseEvent(new RoutedEventArgs(SelectedTabChangedEvent));

			if(tgi != null)
				tgc.SelectedContent = tgi.Content;

		}
	}

	[ContentProperty("Items")]
	public class TabGroup : TabControl
	{
		public object Header { get { return (object)GetValue(HeaderProperty); } set { SetValue(HeaderProperty, value); } }
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(TabGroup));

		public new ObservableCollection<TabGroupItem> Items { get { return (ObservableCollection<TabGroupItem>)GetValue(ItemsProperty); } set { SetValue(ItemsProperty, value); } }
		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<TabGroupItem>), typeof(TabGroup));

		internal new TabGroupControl Parent { get; set; }

		public TabGroup()
		{
			Items = new ObservableCollection<TabGroupItem>();
			SelectionChanged += TabGroup_SelectionChanged;
		}

		void TabGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (Parent == null) return;
			Parent.SelectedTab = SelectedItem as TabGroupItem;
		}
	}

	public class TabGroupItem : TabItem
	{
		
	}
}
