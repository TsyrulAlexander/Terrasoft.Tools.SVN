using System.Windows;
using System.Windows.Controls;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View
{
    public partial class BooleanPropertyControl : UserControl
    {
        public static readonly DependencyProperty BooleanProperty =
            DependencyProperty.Register("Property", typeof(BooleanProperty), typeof(BooleanPropertyControl));

        public BooleanPropertyControl()
        {
            InitializeComponent();
        }

        public BooleanProperty Property {
            get => GetValue(BooleanProperty) as BooleanProperty;
            set => SetValue(BooleanProperty, value);
        }
    }
}