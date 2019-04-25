using System;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Terrasoft.Core.Version;
using Terrasoft.Tools.SvnUI.Ioc;

namespace Terrasoft.Tools.SvnUI
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e) {
			IocKernel.Initialize(new IocConfiguration());
			DispatcherHelper.Initialize();
			base.OnStartup(e);
		}
	}
}
