using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View
{
    public partial class DataBaseTypePropertyControl
    {
        public static readonly DependencyProperty DataBaseTypeProperty =
            DependencyProperty.Register("Property", typeof(EnumProperty<DataBaseType>),
                typeof(DataBaseTypePropertyControl)
            );

        public DataBaseTypePropertyControl()
        {
            SetDataBaseTypeCommand = new RelayCommand<DataBaseType>(SetDataBaseType);
            InitializeComponent();
        }

        public RelayCommand<DataBaseType> SetDataBaseTypeCommand { get; set; }

        public EnumProperty<DataBaseType> Property {
            get => GetValue(DataBaseTypeProperty) as EnumProperty<DataBaseType>;
            set => SetValue(DataBaseTypeProperty, value);
        }

        private void SetDataBaseType(DataBaseType type)
        {
            Property.Value = type;
        }
    }
}