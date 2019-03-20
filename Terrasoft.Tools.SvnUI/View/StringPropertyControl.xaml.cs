using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View {
	public partial class StringPropertyControl : UserControl {
		public static readonly DependencyProperty StringProperty =
			DependencyProperty.Register("Property", typeof(StringProperty), typeof(StringPropertyControl));
		public StringProperty Property {
			get => GetValue(StringProperty) as StringProperty;
			set => SetValue(StringProperty, value);
		}
		public ICommand ValueLostFocus {
			get {
				return (ICommand)GetValue(ValueFocusCommandProperty);
			}
			set {
				SetValue(ValueFocusCommandProperty, value);
			}
		}

		// Using a DependencyProperty as the backing store for NestedCommand.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ValueFocusCommandProperty =
			DependencyProperty.Register("ValueLostFocus", typeof(ICommand), typeof(StringPropertyControl));

		public StringPropertyControl() {
			InitializeComponent();
		}
	}
}
