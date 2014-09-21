using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;

namespace EllipticBit.Controls.WPF
{
	public class TextBox : System.Windows.Controls.TextBox
	{
		public delegate void ValidateEventHandler(object sender, ValidateEventArgs e);
		public static readonly RoutedEvent ValidateEvent = EventManager.RegisterRoutedEvent("Validate", RoutingStrategy.Bubble, typeof(ValidateEventHandler), typeof(TextBox));

		public string LabelText { get { return (string)GetValue(LabelTextProperty); } set { SetValue(LabelTextProperty, value); } }
		public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(TextBox));

		public Brush LabelTextColor { get { return (Brush)GetValue(LabelTextColorProperty); } set { SetValue(LabelTextColorProperty, value); } }
		public static readonly DependencyProperty LabelTextColorProperty = DependencyProperty.Register("LabelTextColor", typeof(Brush), typeof(TextBox));

		public bool IsRequired { get { return (bool)GetValue(IsRequiredProperty); } set { SetValue(IsRequiredProperty, value); } }
		public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(TextBox));

		public bool IsInvalid { get { return (bool)GetValue(IsInvalidProperty); } private set { SetValue(IsInvalidPropertyKey, value); } }
		internal static readonly DependencyPropertyKey IsInvalidPropertyKey = DependencyProperty.RegisterReadOnly("IsInvalid", typeof(bool), typeof(TextBox), new PropertyMetadata(false));
		public static readonly DependencyProperty IsInvalidProperty = IsInvalidPropertyKey.DependencyProperty;

		public bool HasText { get { return (bool)GetValue(HasTextProperty); } private set { SetValue(HasTextPropertyKey, value); } }
		internal static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly("HasText", typeof(bool), typeof(TextBox), new PropertyMetadata(false));
		public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

		static TextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
		}

		public TextBox() : base()
		{
		}

		public event ValidateEventHandler Validate
		{
			add { AddHandler(ValidateEvent, value); }
			remove { RemoveHandler(ValidateEvent, value); }
		}

		public virtual bool OnValidate()
		{
			var args = new ValidateEventArgs(ValidateEvent, this);
			RaiseEvent(args);
			IsInvalid = !args.IsValid;
			return args.IsValid;
		}

		protected override void OnTextChanged(System.Windows.Controls.TextChangedEventArgs e)
		{
			base.OnTextChanged(e);

			HasText = (Text != "");

			OnValidate();
		}

	}

	public class ValidateEventArgs : RoutedEventArgs
	{
		public bool IsValid { get; set; }

		public ValidateEventArgs() : base() { IsValid = true; }
		public ValidateEventArgs(RoutedEvent routedEvent) : base(routedEvent) { IsValid = true; }
		public ValidateEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { IsValid = true; }
	}
}