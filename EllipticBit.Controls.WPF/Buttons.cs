using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EllipticBit.Controls.WPF
{
	public class Button : System.Windows.Controls.Button
	{
		public bool IsRequired { get { return (bool)GetValue(IsRequiredProperty); } set { SetValue(IsRequiredProperty, value); } }
		public static DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(Button));

		public bool IsUrgent { get { return (bool)GetValue(IsUrgentProperty); } set { SetValue(IsUrgentProperty, value); } }
		public static DependencyProperty IsUrgentProperty = DependencyProperty.Register("IsUrgent", typeof(bool), typeof(Button));
	}

	public class PrimaryButton : Button { }

	public class SubtleButton : Button { }

	public class LinkButton : Button { }

	public class DropDownButton : Button
	{
		public object DropDownContent { get { return (object)GetValue(DropDownContentProperty); } set { SetValue(DropDownContentProperty, value); } }
		public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(DropDownButton), new PropertyMetadata(null));

		public bool IsDropDownContentOpen { get { return (bool)GetValue(IsDropDownContentOpenProperty); } set { SetValue(IsDropDownContentOpenProperty, value); } }
		public static readonly DependencyProperty IsDropDownContentOpenProperty = DependencyProperty.Register("IsDropDownContentOpen", typeof(bool), typeof(DropDownButton), new PropertyMetadata(false));

		protected override void OnClick()
		{
			base.OnClick();
			if (DropDownContent != null)
			{
				IsDropDownContentOpen = true;
			}
			else if (ContextMenu != null)
			{
				ContextMenu.Placement = PlacementMode.Bottom;
				ContextMenu.PlacementTarget = this;
				ContextMenu.IsOpen = true;
			}
		}
	}

	public class SplitButton : Button
	{
		public object SplitContent { get { return (object)GetValue(SplitContentProperty); } set { SetValue(SplitContentProperty, value); } }
		public static readonly DependencyProperty SplitContentProperty = DependencyProperty.Register("SplitContent", typeof(object), typeof(SplitButton), new PropertyMetadata(null));

		public bool IsDropDownOpen { get { return (bool)GetValue(IsDropDownOpenProperty); } set { SetValue(IsDropDownOpenProperty, value); } }
		public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(SplitButton), new PropertyMetadata(false, IsDropDownOpenChangedCallback));

		public bool IsSplitContentOpen { get { return (bool)GetValue(IsSplitContentOpenProperty); } set { SetValue(IsSplitContentOpenProperty, value); } }
		public static readonly DependencyProperty IsSplitContentOpenProperty = DependencyProperty.Register("IsSplitContentOpen", typeof(bool), typeof(SplitButton), new PropertyMetadata(false));

		private static void IsDropDownOpenChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sb = o as SplitButton;
			if (sb == null) return;
			if (Convert.ToBoolean(e.NewValue))
			{
				if (sb.SplitContent != null)
					sb.IsSplitContentOpen = true;
				else if (sb.ContextMenu != null)
				{
					sb.ContextMenu.Placement = PlacementMode.Bottom;
					sb.ContextMenu.PlacementTarget = sb;
					sb.ContextMenu.IsOpen = true;
				}
			}
			else
			{
				sb.ContextMenu.IsOpen = false;
				sb.IsSplitContentOpen = false;
			}
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (ContextMenu != null) ContextMenu.IsOpen = false;
			IsSplitContentOpen = false;
		}
	}

	public class GroupButton : Button
	{
		internal enum ButtonLocation
		{
			Only,
			First,
			Middle,
			Last
		}

		public bool IsOnly { get { return (bool)GetValue(IsOnlyProperty); } set { SetValue(IsOnlyProperty, value); } }
		private static readonly DependencyProperty IsOnlyProperty = DependencyProperty.Register("IsOnly", typeof(bool), typeof(GroupButton), new PropertyMetadata(false));

		public bool IsFirst { get { return (bool)GetValue(IsFirstProperty); } set { SetValue(IsFirstProperty, value); } }
		private static readonly DependencyProperty IsFirstProperty = DependencyProperty.Register("IsFirst", typeof(bool), typeof(GroupButton), new PropertyMetadata(false));

		public bool IsMiddle { get { return (bool)GetValue(IsMiddleProperty); } set { SetValue(IsMiddleProperty, value); } }
		private static readonly DependencyProperty IsMiddleProperty = DependencyProperty.Register("IsMiddle", typeof(bool), typeof(GroupButton), new PropertyMetadata(false));

		public bool IsLast { get { return (bool)GetValue(IsLastProperty); } set { SetValue(IsLastProperty, value); } }
		private static readonly DependencyProperty IsLastProperty = DependencyProperty.Register("IsLast", typeof(bool), typeof(GroupButton), new PropertyMetadata(false));

		public Orientation Orientation { get { return (Orientation)GetValue(OrientationProperty); } set { SetValue(OrientationProperty, value); } }
		private static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(GroupButton), new PropertyMetadata(Orientation.Horizontal));

		private new bool IsDefault { get { return (bool)GetValue(IsDefaultProperty); } set { SetValue(IsDefaultProperty, value); } }
		private new static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register("IsDefault", typeof(bool), typeof(GroupButton));

		private new bool IsCancel { get { return (bool)GetValue(IsCancelProperty); } set { SetValue(IsCancelProperty, value); } }
		private new static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof(bool), typeof(GroupButton));

		internal void SetLocation(ButtonLocation Location)
		{
			if (Location == ButtonLocation.Only) { IsOnly = true; IsFirst = false; IsMiddle = false; IsLast = false; }
			if (Location == ButtonLocation.First) { IsOnly = false; IsFirst = true; IsMiddle = false; IsLast = false; }
			if (Location == ButtonLocation.Middle) { IsOnly = false; IsFirst = false; IsMiddle = true; IsLast = false; }
			if (Location == ButtonLocation.Last) { IsOnly = false; IsFirst = false; IsMiddle = false; IsLast = true; }
		}

		internal void SetOrientation(Orientation Orientation)
		{
			this.Orientation = Orientation;
		}
	}

	public class GroupPrimaryButton : GroupButton { }

	public class GroupDropDownButton : GroupButton
	{
		public object DropDownContent { get { return (object)GetValue(DropDownContentProperty); } set { SetValue(DropDownContentProperty, value); } }
		public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register("DropDownContent", typeof(object), typeof(GroupDropDownButton), new PropertyMetadata(null));

		public bool IsDropDownContentOpen { get { return (bool)GetValue(IsDropDownContentOpenProperty); } set { SetValue(IsDropDownContentOpenProperty, value); } }
		public static readonly DependencyProperty IsDropDownContentOpenProperty = DependencyProperty.Register("IsDropDownContentOpen", typeof(bool), typeof(GroupDropDownButton), new PropertyMetadata(false));

		protected override void OnClick()
		{
			base.OnClick();
			if (DropDownContent != null)
			{
				IsDropDownContentOpen = true;
			}
			else if (ContextMenu != null)
			{
				ContextMenu.Placement = PlacementMode.Bottom;
				ContextMenu.PlacementTarget = this;
				ContextMenu.IsOpen = true;
			}
		}
	}

	public class GroupSplitButton : GroupButton
	{
		public object SplitContent { get { return (object)GetValue(SplitContentProperty); } set { SetValue(SplitContentProperty, value); } }
		public static readonly DependencyProperty SplitContentProperty = DependencyProperty.Register("SplitContent", typeof(object), typeof(GroupSplitButton), new PropertyMetadata(null));

		public bool IsDropDownOpen { get { return (bool)GetValue(IsDropDownOpenProperty); } set { SetValue(IsDropDownOpenProperty, value); } }
		public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(GroupSplitButton), new PropertyMetadata(false, IsDropDownOpenChangedCallback));

		public bool IsSplitContentOpen { get { return (bool)GetValue(IsSplitContentOpenProperty); } set { SetValue(IsSplitContentOpenProperty, value); } }
		public static readonly DependencyProperty IsSplitContentOpenProperty = DependencyProperty.Register("IsSplitContentOpen", typeof(bool), typeof(GroupSplitButton), new PropertyMetadata(false));

		private static void IsDropDownOpenChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var sb = o as SplitButton;
			if (sb == null) return;
			if (Convert.ToBoolean(e.NewValue))
			{
				if (sb.SplitContent != null)
					sb.IsSplitContentOpen = true;
				else if (sb.ContextMenu != null)
				{
					sb.ContextMenu.Placement = PlacementMode.Bottom;
					sb.ContextMenu.PlacementTarget = sb;
					sb.ContextMenu.IsOpen = true;
				}
			}
			else
			{
				sb.ContextMenu.IsOpen = false;
				sb.IsSplitContentOpen = false;
			}
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (ContextMenu != null) ContextMenu.IsOpen = false;
			IsSplitContentOpen = false;
		}
	}

	public class GroupToggleButton : GroupButton
	{
		public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GroupToggleButton));
		public event RoutedEventHandler Checked { add { AddHandler(CheckedEvent, value); } remove { RemoveHandler(CheckedEvent, value); } }

		public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GroupToggleButton));
		public event RoutedEventHandler Unchecked { add { AddHandler(UncheckedEvent, value); } remove { RemoveHandler(UncheckedEvent, value); } }

		public bool IsChecked { get { return (bool)GetValue(IsCheckedProperty); } set { SetValue(IsCheckedProperty, value); } }
		public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(GroupToggleButton), new PropertyMetadata(false, IsCheckedChangedCallback));

		private static void IsCheckedChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var de = o as GroupToggleButton;
			if (de == null) return;

			if ((bool)e.NewValue) de.RaiseEvent(new RoutedEventArgs(CheckedEvent));
			else de.RaiseEvent(new RoutedEventArgs(UncheckedEvent));
		}

		protected override void OnClick()
		{
			base.OnClick();
			IsChecked = !IsChecked;
		}
	}
}