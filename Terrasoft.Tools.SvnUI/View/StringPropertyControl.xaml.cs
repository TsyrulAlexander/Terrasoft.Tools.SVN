using System;
using System.Collections.Generic;
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
using Terrasoft.Tools.SvnUI.Model;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View
{
	/// <summary>
	/// Interaction logic for StringPropertyControl.xaml
	/// </summary>
	public partial class StringPropertyControl : UserControl
	{
		public static readonly DependencyProperty StringProperty =
			DependencyProperty.Register("Property", typeof(StringProperty), typeof(StringPropertyControl));
		public StringProperty Property {
			get => GetValue(StringProperty) as StringProperty;
			set => SetValue(StringProperty, value);
		}
		public StringPropertyControl() {
			InitializeComponent();
		}
	}
}
