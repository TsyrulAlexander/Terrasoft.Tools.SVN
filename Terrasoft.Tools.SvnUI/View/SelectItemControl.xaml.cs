using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View
{
    public partial class SelectItemControl : UserControl
    {
        public static readonly DependencyProperty StringProperty =
            DependencyProperty.Register("Property", typeof(StringProperty), typeof(SelectItemControl));

        public static readonly DependencyProperty OpenCommandProperty =
            DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(SelectItemControl));

        public SelectItemControl()
        {
            InitializeComponent();
        }

        public StringProperty Property {
            get => GetValue(StringProperty) as StringProperty;
            set => SetValue(StringProperty, value);
        }

        public ICommand OpenCommand {
            get => (ICommand) GetValue(OpenCommandProperty);
            set => SetValue(OpenCommandProperty, value);
        }
    }
}