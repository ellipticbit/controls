using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EllipticBit.Controls.WPF.Dialogs
{
	public class DialogService
	{
		private static bool IsProcessingMessage { get; set; }

		public static ConcurrentQueue<Dialog> Messages { get; private set; }
		public static DialogViewer Viewer { get; private set; }
		internal static string DefaultModuleTitle { get; private set; }

		static DialogService()
		{
			Messages = new ConcurrentQueue<Dialog>();
			DefaultModuleTitle = "";
		}

		public static void Initialize(string DefaultModuleTitle, DialogViewer Viewer)
		{
			DialogService.DefaultModuleTitle = DefaultModuleTitle;
			DialogService.Viewer = Viewer;
		}

		internal static async void ProcessNextMessage()
		{
			if (Application.Current.Dispatcher.CheckAccess())
				InternalProcessNextMessage();
			else
				await Application.Current.Dispatcher.InvokeAsync(InternalProcessNextMessage, System.Windows.Threading.DispatcherPriority.Normal);
		}

		private static void InternalProcessNextMessage()
		{
			if (IsProcessingMessage) return;
			IsProcessingMessage = true;

			Dialog next = null;
			if (Messages.TryDequeue(out next) == false)
			{
				IsProcessingMessage = false;
				Viewer.ActiveDialog = null;
				return;
			}

			if (next != null)
			{
				Viewer.ActiveDialog = next;
				next.Focus();
				next.SetFocus();
			}
		}

		public static void CloseActiveMessageBox()
		{
			IsProcessingMessage = false;

			ProcessNextMessage();
		}

		public static void ShowMessageDialog(object Module, string Caption, string Message)
		{
			Messages.Enqueue(new MessageDialog(Module, Caption, Message, new[] { new DialogAction("OK", true, true) }));
			ProcessNextMessage();
		}

		public static void ShowMessageDialog(object Module, string Caption, string Message, params DialogAction[] Actions)
		{
			Messages.Enqueue(new MessageDialog(Module, Caption, Message, Actions));
			ProcessNextMessage();
		}

		public static void ShowContentDialog(object Module, string Caption, IDialogContent Content)
		{
			Messages.Enqueue(new ContentDialog(Module, Caption, Content, null));
			ProcessNextMessage();
		}

		public static void ShowContentDialog(object Module, string Caption, IDialogContent Content, params DialogAction[] Actions)
		{
			Messages.Enqueue(new ContentDialog(Module, Caption, Content, Actions));
			ProcessNextMessage();
		}

	}

	public abstract class Dialog : System.Windows.Controls.ContentControl
	{
		//Dialog Properties
		public string Module { get { return (string)GetValue(ModuleProperty); } set { SetValue(ModuleProperty, value); } }
		public static readonly DependencyProperty ModuleProperty = DependencyProperty.Register("Module", typeof(string), typeof(Dialog));

		public string Caption { get { return (string)GetValue(CaptionProperty); } set { SetValue(CaptionProperty, value); } }
		public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(Dialog));

		public ObservableCollection<DialogAction> Actions { get { return (ObservableCollection<DialogAction>)GetValue(ActionsProperty); } set { SetValue(ActionsProperty, value); } }
		public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ObservableCollection<DialogAction>), typeof(Dialog));

		public abstract void SetFocus();
	}

	public class DialogAction : System.Windows.Controls.Button
	{
		public Action Action { get; private set; }
		public bool DoNotClose { get; private set; }

		public DialogAction(string Title, bool IsDefault = false, bool IsCancel = false, bool DoNotClose = false)
		{
			Content = Title ?? DialogService.DefaultModuleTitle;
			this.IsDefault = IsDefault;
			this.IsCancel = IsCancel;
			this.DoNotClose = DoNotClose;
			Action = null;
			DataContext = this;
		}

		public DialogAction(string Title, Action Action, bool IsDefault = false, bool IsCancel = false, bool DoNotClose = false)
		{
			Content = Title ?? DialogService.DefaultModuleTitle;
			this.IsDefault = IsDefault;
			this.IsCancel = IsCancel;
			this.DoNotClose = DoNotClose;
			this.Action = Action;
			DataContext = this;
		}

		protected override void OnClick()
		{
			base.OnClick();

			PerformClick();
		}

		internal void PerformClick()
		{
			if (Action != null) Action();

			if (!DoNotClose)
				DialogService.CloseActiveMessageBox();
		}
	}
}