using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EllipticBit.Controls.WPF
{
	[ContentProperty("Buttons")]
	public class GroupButtonPanel : StackPanel
	{
		static GroupButtonPanel()
		{
			OrientationProperty.OverrideMetadata(typeof(GroupButtonPanel), new FrameworkPropertyMetadata(Orientation.Horizontal, OrientationChangedCallback));
		}

		public GroupButtonPanel()
		{
			Buttons = new ObservableCollection<GroupButton>();
			Buttons.CollectionChanged += Buttons_CollectionChanged;
		}

		private void Buttons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (Buttons.Count == 0) { InternalChildren.Clear(); return; }
			if (Buttons.Count == 1) Buttons[0].SetLocation(GroupButton.ButtonLocation.Only);
			else
			{
				Buttons[0].SetLocation(GroupButton.ButtonLocation.First);
				Buttons[Buttons.Count - 1].SetLocation(GroupButton.ButtonLocation.Last);
				for (int i = 1; i < Buttons.Count - 1; i++)
					Buttons[i].SetLocation(GroupButton.ButtonLocation.Middle);
			}

			if (e.Action == NotifyCollectionChangedAction.Add)
				foreach (var t in e.NewItems)
					InternalChildren.Add((UIElement)t);
			if (e.Action == NotifyCollectionChangedAction.Remove)
				foreach (var t in e.OldItems)
					InternalChildren.Remove((UIElement)t);
		}

		public ObservableCollection<GroupButton> Buttons { get { return (ObservableCollection<GroupButton>)GetValue(ButtonsProperty); } set { SetValue(ButtonsProperty, value); } }
		public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(ObservableCollection<GroupButton>), typeof(GroupButtonPanel));

		private new UIElementCollection Children { get; set; }

		private static void OrientationChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var de = o as GroupButtonPanel;
			if (de == null) return;

			foreach (var t in de.Buttons)
				t.SetOrientation((Orientation)e.NewValue);

			var sp = de as StackPanel;
			sp.Orientation = (Orientation)e.NewValue;
		}
	}
}