using System;
using System.Threading.Tasks;
using System.Windows;

namespace EllipticBit.Controls.WPF.Dialogs
{
	public class DialogActionBase : System.Windows.Controls.Button { }

	public class DialogAction<T> : DialogActionBase
	{
		public Func<Task<T>> Action { get; private set; }
		internal TaskCompletionSource<T> Completion { get; set; }

		protected DialogAction(string title, bool isDefault = false, bool isCancel = false)
		{
			Content = title;
			this.IsDefault = isDefault;
			this.IsCancel = isCancel;
			DataContext = this;

			this.SetResourceReference(StyleProperty, typeof(DialogActionBase));
		}

		public DialogAction(string title, Func<Task<T>> action, bool isDefault = false, bool isCancel = false)
		{
			Content = title;
			this.IsDefault = isDefault;
			this.IsCancel = isCancel;
			DataContext = this;
			this.Action = action;

			this.SetResourceReference(StyleProperty, typeof(DialogActionBase));
		}

		protected override async void OnClick()
		{
			base.OnClick();

			await PerformClick().ConfigureAwait(true);
		}

		internal async Task PerformClick()
		{
			if (Action != null)
			{
				if (Application.Current.Dispatcher.CheckAccess())
				{
					var result = await Action().ConfigureAwait(true);
					Completion.SetResult(result);
				}
				else
				{
					var t = await Application.Current.Dispatcher.InvokeAsync(Action, System.Windows.Threading.DispatcherPriority.Normal).Task.ConfigureAwait(true);
					var result = await t.ConfigureAwait(true);
					Completion.SetResult(result);
				}
			}

			DialogService.CloseActiveMessageBox();
		}
	}
}
