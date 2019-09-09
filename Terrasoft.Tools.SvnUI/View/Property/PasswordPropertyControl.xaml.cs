using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View.Property {

	public partial class PasswordPropertyControl : UserControl {
		public static readonly DependencyProperty StringProperty =
			DependencyProperty.Register("Property", typeof(StringProperty), typeof(PasswordPropertyControl), new PropertyMetadata(PropertyChangedCallback));

		private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			if (e.NewValue != null && e.NewValue is StringProperty property) {
				((PasswordPropertyControl)d).psw.Password = property.Value;
			}
		}

		public StringProperty Property {
			get => GetValue(StringProperty) as StringProperty;
			set => SetValue(StringProperty, value);
		}

		public PasswordPropertyControl() {
			InitializeComponent();
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(Property.Value)) {
				psw.Password = Property.Value;
			}
		}

		private void OnPasswordChanged(object sender, RoutedEventArgs e) {
			Property.Value = ((PasswordBox) sender).Password;
		}
	}
}