using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View.Property {
	public partial class DataBaseTypePropertyControl : UserControl {
		public static readonly DependencyProperty DataBaseTypeProperty =
			DependencyProperty.Register("Property", typeof(EnumProperty<DataBaseType>), typeof(DataBaseTypePropertyControl));
		public RelayCommand<DataBaseType> SetDataBaseTypeCommand { get; set; }
		public EnumProperty<DataBaseType> Property {
			get => GetValue(DataBaseTypeProperty) as EnumProperty<DataBaseType>;
			set => SetValue(DataBaseTypeProperty, value);
		}
		public DataBaseTypePropertyControl() {
			SetDataBaseTypeCommand = new RelayCommand<DataBaseType>(SetDataBaseType);
			InitializeComponent();
		}

		private void SetDataBaseType(DataBaseType type) {
			Property.Value = type;
		}
	}
}
