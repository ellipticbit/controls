using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EllipticBit.Controls.WPF.Dialogs
{
	public abstract class DialogBase : DependencyObject
	{
		public string Title { get { return (string)GetValue(TitleProperty); } set { SetValue(TitleProperty, value); } }
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(DialogBase));

		public object Content { get { return (object)GetValue(ContentProperty); } set { SetValue(ContentProperty, value); } }
		public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(DialogBase));

		internal abstract Task DoCancelAction();
		internal abstract Task DoDefaultAction();
	}

	public abstract class Dialog<T> : DialogBase
	{
		public List<DialogAction<T>> Actions { get { return (List<DialogAction<T>>)GetValue(ActionsProperty); } set { SetValue(ActionsProperty, value); } }
		public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(List<DialogAction<T>>), typeof(Dialog<T>));

		public Dialog(object content)
		{
			Actions = new List<DialogAction<T>>();
			Content = content;
		}

		internal sealed override async Task DoCancelAction()
		{
			foreach (var a in Actions.Where(a => a.IsCancel))
			{
				await a.PerformClick().ConfigureAwait(true);
			}
		}

		internal sealed override async Task DoDefaultAction()
		{
			foreach (var a in Actions.Where(a => a.IsDefault))
			{
				await a.PerformClick().ConfigureAwait(true);
			}
		}
	}

	internal sealed class MessageDialog<T> : Dialog<T>
	{
		public MessageDialog(string title, string message, DialogAction<T>[] actions)
			: base(message)
		{
			Title = title;
			Actions.AddRange(actions);
		}
	}
}
