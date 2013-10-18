﻿using System;
using System.Windows;
using System.Windows.Controls;
using DeltaEngine.Core;
using DeltaEngine.Extensions;

namespace DeltaEngine.Editor.AppBuilder
{
	/// <summary>
	/// Shows all available apps provided by the BuiltAppsListViewModel.
	/// </summary>
	public partial class BuiltAppsListView
	{
		public BuiltAppsListView()
		{
			InitializeComponent();
		}

		public BuiltAppsListViewModel ViewModel
		{
			get { return viewModel; }
			set
			{
				viewModel = value;
				DataContext = value;
			}
		}
		private BuiltAppsListViewModel viewModel;

		private void OnRebuildAppClicked(object rebuildButton, RoutedEventArgs e)
		{
			AppInfo boundApp = GetBoundApp(rebuildButton);
			viewModel.RequestRebuild(boundApp);
		}

		private static AppInfo GetBoundApp(object clickedButton)
		{
			Panel controlOwner = GetControlOwner(clickedButton);
			return GetBoundApp(controlOwner);
		}

		private static Panel GetControlOwner(object clickedControl)
		{
			return (clickedControl as Control).Parent as Panel;
		}

		private static AppInfo GetBoundApp(FrameworkElement controlOwner)
		{
			return controlOwner.DataContext as AppInfo;
		}

		private void OnLaunchAppClicked(object launchButton, RoutedEventArgs e)
		{
			Panel controlOwner = GetControlOwner(launchButton);
			AppInfo selectedApp = GetBoundApp(controlOwner);
			(launchButton as Control).IsEnabled = false;
			Action enableLaunchButtonAgain =
				() => Dispatcher.BeginInvoke(new Action(() => (launchButton as Control).IsEnabled = true));
			ThreadExtensions.Start(() => TryLaunchApp(selectedApp, enableLaunchButtonAgain));
		}

		private static void TryLaunchApp(AppInfo selectedApp, Action enableLaunchButtonAgain)
		{
			try
			{
				selectedApp.LaunchAppOnPrimaryDevice();
			}
			catch (AppInfo.NoDeviceAvailable)
			{
				Logger.Warning("No " + selectedApp.Platform + " device found. Please make sure your" +
					" device is connected and you have the correct driver installed.");
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
			enableLaunchButtonAgain();
		}
	}
}
