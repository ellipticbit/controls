using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public partial class MessageDialog : Dialog
	{
		public string Message { get { return (string)GetValue(MessageProperty); } set { SetValue(MessageProperty, value); } }
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(MessageDialog));

		public MessageDialog()
		{
			Module = DialogService.DefaultModuleTitle;
			Actions = new ObservableCollection<DialogAction>();

			InitializeComponent();
		}

		public MessageDialog(object Module, string Caption, string Message, DialogAction[] Actions)
		{
			this.Module = Module == null ? DialogService.DefaultModuleTitle : Module.ToString();
			this.Caption = Caption;
			this.Message = Message;
			this.Actions = new ObservableCollection<DialogAction>(Actions);

			InitializeComponent();
		}

		private void Dialog_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				foreach (DialogAction a in Actions.Where(a => a.IsCancel))
				{
					a.PerformClick();
					e.Handled = true;
				}
			if (e.Key == Key.Enter)
				foreach (DialogAction a in Actions.Where(a => a.IsDefault))
				{
					a.PerformClick();
					e.Handled = true;
				}
		}

		public override void SetFocus() {}
	}
}