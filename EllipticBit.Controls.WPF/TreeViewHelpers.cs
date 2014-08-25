using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;

namespace EllipticBit.Controls.WPF
{
	public static class TreeViewHelper
	{
		//
		// The TreeViewItem that the mouse is currently directly over (or null).
		//
		private static TreeViewItem _currentItem = null;

		//
		// IsMouseDirectlyOverItem:  A DependencyProperty that will be true only on the 
		// TreeViewItem that the mouse is directly over.  I.e., this won't be set on that 
		// parent item.
		//
		// This is the only public member, and is read-only.
		//

		// The property key (since this is a read-only DP)
		private static readonly DependencyPropertyKey IsMouseDirectlyOverItemKey =
			DependencyProperty.RegisterAttachedReadOnly("IsMouseDirectlyOverItem",
												typeof(bool),
												typeof(TreeViewHelper),
												new FrameworkPropertyMetadata(null, new CoerceValueCallback(CalculateIsMouseDirectlyOverItem)));

		// The DP itself
		public static readonly DependencyProperty IsMouseDirectlyOverItemProperty =
			IsMouseDirectlyOverItemKey.DependencyProperty;

		// A strongly-typed getter for the property.
		public static bool GetIsMouseDirectlyOverItem(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsMouseDirectlyOverItemProperty);
		}

		// A coercion method for the property
		private static object CalculateIsMouseDirectlyOverItem(DependencyObject item, object value)
		{
			// This method is called when the IsMouseDirectlyOver property is being calculated
			// for a TreeViewItem.  

			if (item == _currentItem)
				return true;
			else
				return false;
		}

		//
		// UpdateOverItem:  A private RoutedEvent used to find the nearest encapsulating
		// TreeViewItem to the mouse's current position.
		//

		private static readonly RoutedEvent UpdateOverItemEvent = EventManager.RegisterRoutedEvent(
			"UpdateOverItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewHelper));

		//
		// Class constructor
		//

		static TreeViewHelper()
		{
			// Get all Mouse enter/leave events for TreeViewItem.
			EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.MouseEnterEvent, new MouseEventHandler(OnMouseTransition), true);
			EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.MouseLeaveEvent, new MouseEventHandler(OnMouseTransition), true);

			// Listen for the UpdateOverItemEvent on all TreeViewItem's.
			EventManager.RegisterClassHandler(typeof(TreeViewItem), UpdateOverItemEvent, new RoutedEventHandler(OnUpdateOverItem));
		}


		//
		// OnUpdateOverItem:  This method is a listener for the UpdateOverItemEvent.  When it is received,
		// it means that the sender is the closest TreeViewItem to the mouse (closest in the sense of the tree,
		// not geographically).

		static void OnUpdateOverItem(object sender, RoutedEventArgs args)
		{
			// Mark this object as the tree view item over which the mouse
			// is currently positioned.
			_currentItem = sender as TreeViewItem;

			// Tell that item to re-calculate the IsMouseDirectlyOverItem property
			_currentItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);

			// Prevent this event from notifying other tree view items higher in the tree.
			args.Handled = true;
		}

		//
		// OnMouseTransition:  This method is a listener for both the MouseEnter event and
		// the MouseLeave event on TreeViewItems.  It updates the _currentItem, and updates
		// the IsMouseDirectlyOverItem property on the previous TreeViewItem and the new
		// TreeViewItem.

		static void OnMouseTransition(object sender, MouseEventArgs args)
		{
			lock (IsMouseDirectlyOverItemProperty)
			{
				if (_currentItem != null)
				{
					// Tell the item that previously had the mouse that it no longer does.
					DependencyObject oldItem = _currentItem;
					_currentItem = null;
					oldItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);
				}

				// Get the element that is currently under the mouse.
				IInputElement currentPosition = Mouse.DirectlyOver;

				// See if the mouse is still over something (any element, not just a tree view item).
				if (currentPosition != null)
				{
					// Yes, the mouse is over something.
					// Raise an event from that point.  If a TreeViewItem is anywhere above this point
					// in the tree, it will receive this event and update _currentItem.

					RoutedEventArgs newItemArgs = new RoutedEventArgs(UpdateOverItemEvent);
					currentPosition.RaiseEvent(newItemArgs);

				}
			}
		}

		public static TreeViewItem ContainerFromItem(this TreeView treeView, object item)
		{
			TreeViewItem containerThatMightContainItem = (TreeViewItem)treeView.ItemContainerGenerator.ContainerFromItem(item);
			if (containerThatMightContainItem != null)
				return containerThatMightContainItem;
			else
				return ContainerFromItem(treeView.ItemContainerGenerator, treeView.Items, item);
		}

		private static TreeViewItem ContainerFromItem(ItemContainerGenerator parentItemContainerGenerator, ItemCollection itemCollection, object item)
		{
			foreach (object curChildItem in itemCollection)
			{
				TreeViewItem parentContainer = (TreeViewItem)parentItemContainerGenerator.ContainerFromItem(curChildItem);
				if (parentContainer == null)
					return null;
				TreeViewItem containerThatMightContainItem = (TreeViewItem)parentContainer.ItemContainerGenerator.ContainerFromItem(item);
				if (containerThatMightContainItem != null)
					return containerThatMightContainItem;
				TreeViewItem recursionResult = ContainerFromItem(parentContainer.ItemContainerGenerator, parentContainer.Items, item);
				if (recursionResult != null)
					return recursionResult;
			}
			return null;
		}

		public static object ItemFromContainer(this TreeView treeView, TreeViewItem container)
		{
			TreeViewItem itemThatMightBelongToContainer = (TreeViewItem)treeView.ItemContainerGenerator.ItemFromContainer(container);
			if (itemThatMightBelongToContainer != null)
				return itemThatMightBelongToContainer;
			else
				return ItemFromContainer(treeView.ItemContainerGenerator, treeView.Items, container);
		}

		private static object ItemFromContainer(ItemContainerGenerator parentItemContainerGenerator, ItemCollection itemCollection, TreeViewItem container)
		{
			foreach (object curChildItem in itemCollection)
			{
				TreeViewItem parentContainer = (TreeViewItem)parentItemContainerGenerator.ContainerFromItem(curChildItem);
				if (parentContainer == null)
					return null;
				TreeViewItem itemThatMightBelongToContainer = (TreeViewItem)parentContainer.ItemContainerGenerator.ItemFromContainer(container);
				if (itemThatMightBelongToContainer != null)
					return itemThatMightBelongToContainer;
				TreeViewItem recursionResult = ItemFromContainer(parentContainer.ItemContainerGenerator, parentContainer.Items, container) as TreeViewItem;
				if (recursionResult != null)
					return recursionResult;
			}
			return null;
		}
	}

	public static class TreeViewItemExtensions
	{
		public static int GetDepth(this TreeViewItem item)
		{
			FrameworkElement elem = item;
			var parent = VisualTreeHelper.GetParent(item);
			var count = 0;
			while (parent != null && !(parent is TreeView))
			{
				var tvi = parent as TreeViewItem; if (parent is TreeViewItem)
					count++; parent = VisualTreeHelper.GetParent(parent);
			}
			return count;
		}
	}

	public class TreeViewMarginMultiplierConverter : IValueConverter
	{
		public double Length { get; set; }
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var item = value as TreeViewItem;
			if (item == null)
				return new Thickness(0);
			return new Thickness(Length * item.GetDepth(), 0, 0, 0);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new System.NotImplementedException();
		}
	}
}