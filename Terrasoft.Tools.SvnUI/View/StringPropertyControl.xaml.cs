using System.Windows;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View
{
    public partial class StringPropertyControl
    {
        public static readonly DependencyProperty StringProperty =
            DependencyProperty.Register("Property", typeof(StringProperty), typeof(StringPropertyControl));

        public StringPropertyControl() {
            InitializeComponent();
        }

        public StringProperty Property {
            get => GetValue(StringProperty) as StringProperty;
            set => SetValue(StringProperty, value);
        }
    }
}