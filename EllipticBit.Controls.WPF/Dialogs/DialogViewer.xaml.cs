using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EllipticBit.Controls.WPF.Dialogs
{
	public partial class DialogViewer : UserControl
	{
		public Dialog ActiveDialog { get { return (Dialog)GetValue(ActiveDialogProperty); } set { SetValue(ActiveDialogProperty, value); if (value == null) { Visibility = System.Windows.Visibility.Collapsed; } else { Visibility = System.Windows.Visibility.Visible; } } }
		public static readonly DependencyProperty ActiveDialogProperty = DependencyProperty.Register("ActiveDialog", typeof(Dialog), typeof(DialogViewer), new PropertyMetadata(ActiveDialogChangedCallback));

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

		public void SetMaxSize(double Height, double Width)
		{
			maxContentHeight = Height - 100;
			maxContentWidth = Width - 100;
			if (ActiveDialog == null) return;
			ActiveDialog.MaxHeight = maxContentHeight;
			ActiveDialog.MaxWidth = maxContentWidth;
		}
	}
}