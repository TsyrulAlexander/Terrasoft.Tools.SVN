using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Terrasoft.Tools.SvnUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseLogPanelClick(object sender, RoutedEventArgs e)
        {
            LogListBox.Visibility = Visibility.Collapsed;
            DrawerHost.CloseDrawerCommand.Execute(Dock.Bottom, (IInputElement) sender);
        }

        private void OpenLogPanelClick(object sender, RoutedEventArgs e)
        {
            LogListBox.Visibility = Visibility.Visible;
            DrawerHost.OpenDrawerCommand.Execute(Dock.Bottom, (IInputElement) sender);
        }
    }
}