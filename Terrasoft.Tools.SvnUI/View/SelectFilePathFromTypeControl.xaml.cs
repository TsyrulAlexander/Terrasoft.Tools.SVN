using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.CommandWpf;
using Terrasoft.Tools.SvnUI.Model.Enums;
using Terrasoft.Tools.SvnUI.Model.Property;

namespace Terrasoft.Tools.SvnUI.View {
	public partial class SelectFilePathFromTypeControl : UserControl {
		public static readonly DependencyProperty FilePathTypeProperty = DependencyProperty.Register("FilePathType",
			typeof(EnumProperty<FilePathType>), typeof(SelectFilePathFromTypeControl));
		public static readonly DependencyProperty LocalPathProperty =
			DependencyProperty.Register("LocalPath", typeof(StringProperty),
				typeof(SelectFilePathFromTypeControl));
		public static readonly DependencyProperty FtpFilePathProperty =
			DependencyProperty.Register("FtpFilePath", typeof(StringProperty), typeof(SelectFilePathFromTypeControl));
		public static readonly DependencyProperty FtpLoginProperty =
			DependencyProperty.Register("FtpLogin", typeof(StringProperty), typeof(SelectFilePathFromTypeControl));
		public static readonly DependencyProperty FtpPasswordProperty =
			DependencyProperty.Register("FtpPassword", typeof(StringProperty), typeof(SelectFilePathFromTypeControl));
		public static readonly DependencyProperty FtpTempFileProperty =
			DependencyProperty.Register("FtpTempFile", typeof(StringProperty), typeof(SelectFilePathFromTypeControl));
		public EnumProperty<FilePathType> FilePathType {
			get => GetValue(FilePathTypeProperty) as EnumProperty<FilePathType>;
			set => SetValue(FilePathTypeProperty, value);
		}
		public StringProperty LocalPath {
			get => GetValue(LocalPathProperty) as StringProperty;
			set => SetValue(LocalPathProperty, value);
		}
		public StringProperty FtpFilePath {
			get => GetValue(FtpFilePathProperty) as StringProperty;
			set => SetValue(FtpFilePathProperty, value);
		}
		public StringProperty FtpLogin {
			get => GetValue(FtpLoginProperty) as StringProperty;
			set => SetValue(FtpLoginProperty, value);
		}
		public StringProperty FtpPassword {
			get => GetValue(FtpPasswordProperty) as StringProperty;
			set => SetValue(FtpPasswordProperty, value);
		}
		public StringProperty FtpTempFile {
			get => GetValue(FtpTempFileProperty) as StringProperty;
			set => SetValue(FtpTempFileProperty, value);
		}
		public RelayCommand<FilePathType> SetFilePathTypeCommand {
			get; set;
		}

		public SelectFilePathFromTypeControl() {
			SetFilePathTypeCommand = new RelayCommand<FilePathType>(SetFilePathType);
			InitializeComponent();
		}

		private void SetFilePathType(FilePathType type) {
			FilePathType.Value = type;
		}
	}
}
