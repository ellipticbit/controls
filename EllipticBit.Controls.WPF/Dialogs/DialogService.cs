using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace EllipticBit.Controls.WPF.Dialogs
{
	public class DialogService
	{
		private static bool IsProcessingMessage { get; set; }

		private static ConcurrentQueue<DialogBase> Messages { get; }
		private static DialogViewer Viewer { get; set; }

		static DialogService()
		{
			Messages = new ConcurrentQueue<DialogBase>();
		}

		public static void Initialize(DialogViewer Viewer)
		{
			DialogService.Viewer = Viewer;
		}

		private static void ProcessNextMessage()
		{
			if (IsProcessingMessage) return;
			IsProcessingMessage = true;

			DialogBase next = null;
			if (Messages.TryDequeue(out next) == false)
			{
				IsProcessingMessage = false;
				Viewer.ActiveDialog = null;
				return;
			}

			if (next != null)
			{
				Viewer.ActiveDialog = next;
			}
		}

		internal static void CloseActiveMessageBox()
		{
			IsProcessingMessage = false;

			ProcessNextMessage();
		}

		public static void ShowMessageDialog(string title, string message)
		{
			Messages.Enqueue(new MessageDialog<bool>(title, message, new[] { new DialogAction<bool>("OK", null, true, true) }));
			ProcessNextMessage();
		}

		public static Task<T> ShowMessageDialog<T>(string title, string message, params DialogAction<T>[] actions)
		{
			var dialog = new MessageDialog<T>(title, message, actions);

			var tsc = new TaskCompletionSource<T>();
			foreach (var da in dialog.Actions)
				da.Completion = tsc;

			Messages.Enqueue(dialog);
			ProcessNextMessage();

			return tsc.Task;
		}

		public static Task<T> ShowContentDialog<T>(Dialog<T> dialog)
		{
			var tsc = new TaskCompletionSource<T>();
			foreach (var da in dialog.Actions)
				da.Completion = tsc;

			Messages.Enqueue(dialog);
			ProcessNextMessage();

			return tsc.Task;
		}
	}
}