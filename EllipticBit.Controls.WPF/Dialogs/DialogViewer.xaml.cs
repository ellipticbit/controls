using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EllipticBit.Controls.WPF.Dialogs
{
	public partial class DialogViewer : Grid
	{
		public DialogBase ActiveDialog { get { return (DialogBase)GetValue(ActiveDialogProperty); } set { SetValue(ActiveDialogProperty, value); if (value == null) { Visibility = System.Windows.Visibility.Collapsed; } else { Visibility = System.Windows.Visibility.Visible; } } }
		public static readonly DependencyProperty ActiveDialogProperty = DependencyProperty.Register("ActiveDialog", typeof(DialogBase), typeof(DialogViewer), new PropertyMetadata(ActiveDialogChangedCallback));

		private static void ActiveDialogChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var de = o as DialogViewer;
			if (de == null || de.ActiveDialog == null) return;
			
			de.ActiveDialog.MaxHeight = de.maxContentHeight;
			de.ActiveDialog.MaxWidth = de.maxContentWidth;
		}

		private double maxContentHeight;
		private double maxContentWidth;

		public DialogViewer()
		{
			InitializeComponent();
		}

		private void DialogViewer_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			maxContentHeight = e.NewSize.Height - 100;
			maxContentWidth = e.NewSize.Width - 100;
			if (ActiveDialog == null) return;
			ActiveDialog.MaxHeight = maxContentHeight;
			ActiveDialog.MaxWidth = maxContentWidth;
		}

		private async void DialogViewer_OnPreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				await ActiveDialog.DoCancelAction().ConfigureAwait(true);
			if (e.Key == Key.Enter)
				await ActiveDialog.DoDefaultAction().ConfigureAwait(true);
		}
	}
}