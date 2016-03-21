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
using System.Windows.Threading;

namespace EllipticBit.Controls.WPF.Dialogs
{
	public partial class ContentDialog : Dialog
	{
		public object Message { get { return GetValue(MessageProperty); } set { SetValue(MessageProperty, value); } }
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(object), typeof(ContentDialog));

		public IDialogContent content { get; set; }

		public ContentDialog()
		{
			Module = DialogService.DefaultModuleTitle;
			Caption = Caption;
			Message = Message;
			if (Actions != null)
				Actions = new ObservableCollection<DialogAction>();

			InitializeComponent();
		}

		public ContentDialog(object Module, string Caption, IDialogContent Message)
		{
			this.Module = Module?.ToString() ?? DialogService.DefaultModuleTitle;
			this.Caption = Caption;
			this.Message = Message;
			content = Message;

			InitializeComponent();

			Actions = Message.Actions ?? new ObservableCollection<DialogAction>();
			Message.Host = this;
		}

		private void Dialog_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && Actions != null)
				foreach (DialogAction a in Actions.Where(a => a.IsCancel))
				{
					a.PerformClick();
					e.Handled = true;
				}
			if (e.Key == Key.Enter && Actions != null)
				foreach (DialogAction a in Actions.Where(a => a.IsDefault))
				{
					a.PerformClick();
					e.Handled = true;
				}
		}

		public override async void SetFocus()
		{
			await Dispatcher.InvokeAsync(() => content.SetFocus(), DispatcherPriority.ApplicationIdle);
		}
	}

	public interface IDialogContent
	{
		ContentDialog Host { get; set; }
		ObservableCollection<DialogAction> Actions { get; }
		void SetFocus();
	}
}