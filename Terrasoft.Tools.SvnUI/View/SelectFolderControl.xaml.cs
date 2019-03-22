using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View {
	public partial class SelectFolderControl : UserControl {
		public static readonly DependencyProperty StringProperty =
			DependencyProperty.Register("Property", typeof(StringProperty), typeof(SelectFolderControl));
		public StringProperty Property {
			get => GetValue(StringProperty) as StringProperty;
			set => SetValue(StringProperty, value);
		}

		public ICommand OpenCommand {
			get => (ICommand) GetValue(OpenCommandProperty);
			set => SetValue(OpenCommandProperty, value);
		}
		public static readonly DependencyProperty OpenCommandProperty =
			DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(SelectFolderControl));

		public SelectFolderControl() {
			InitializeComponent();
		}
	}
}