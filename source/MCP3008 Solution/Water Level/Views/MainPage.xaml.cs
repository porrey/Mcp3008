// Copyright © 2015 Daniel Porrey
//
// This file is part of the MCP3008/Water Sensor solution.
// 
// MCP3008/Water Sensor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MCP3008/Water Sensor is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MCP3008/Water Sensor. If not, see http://www.gnu.org/licenses/.
//
using System;
using System.Threading.Tasks;
using Porrey.Common;
using Porrey.WaterLevel.Common;
using Windows.Devices.Sensors;
using Windows.Devices.Sensors.Calibration;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Porrey.WaterLevel.Views
{
	public sealed partial class MainPage : BindablePage
	{
		private DispatcherTimer _timer = null;
		private Mcp3008 _mcp3008 = null;
		private MyApplicationSettings _applicationSettings = null;
		private NonLinearCalibration _calibration = null;

		public MainPage()
		{
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);
			_timer.Tick += Timer_Tick;

			this.InitializeComponent();
		}

		private string _waterLevelDisplayValue = MagicValue.Defaults.WaterLevelDisplay;
		public string WaterLevelDisplayValue
		{
			get
			{
				return _waterLevelDisplayValue;
			}

			set
			{
				this.SetProperty(ref _waterLevelDisplayValue, value);
			}
		}

		private int _waterLevelValue = MagicValue.Defaults.WaterLevelValue;
		public int WaterLevelValue
		{
			get
			{
				return _waterLevelValue;
			}
			set
			{
				this.SetProperty(ref _waterLevelValue, value);
			}
		}

		public MyApplicationSettings ApplicationSettings
		{
			get
			{
				if (_applicationSettings == null)
				{
					_applicationSettings = new MyApplicationSettings();
				}

				return _applicationSettings;
			}
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// ***
			// *** Get the calibration value from application settings
			// ***
			_calibration = new NonLinearCalibration(MagicValue.Defaults.MinimumDepth, MagicValue.Defaults.MaximumDepth, this.ApplicationSettings.CalibrationPoints);

			// ***
			// *** Initialize the MCP3008
			// ***
			_mcp3008 = new Mcp3008(0);
			await _mcp3008.Initialize();

			// ***
			// *** Start the timer
			// ***
			_timer.Start();
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			// ***
			// *** Stop the timer
			// ***
			_timer.Stop();

			// ***
			// *** Dispose the MCP3008
			// ***
			if (_mcp3008 != null)
			{
				_mcp3008.Dispose();
				_mcp3008 = null;
			}

			base.OnNavigatedFrom(e);
		}

		private async Task UpdateWaterLevelUI(float x)
		{
			if (this.Dispatcher != null)
			{
				await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					// ***
					// *** Get the depth in inches
					// ***
					float y = _calibration.AdjustedReading(x);

					// ***
					// *** Convert to a string for display
					// ***
					this.WaterLevelDisplayValue = y.ToString(MagicValue.Format.WaterLevelFormat);

					// ***
					// *** The gauge wants a value between 0 and 360
					// ***
					this.WaterLevelValue = y.Normalize(MagicValue.Defaults.MaximumDepth).AsRange(0, MagicValue.Defaults.MaximumGaugeValue);
				});
			}
		}

		private async void Timer_Tick(object sender, object e)
		{
			if (_mcp3008 != null)
			{
				// ***
				// *** Update the voltage (scale the value so it is a reading
				// *** between 0 and 3.301v)
				// ***
				float value = _mcp3008.Read(Mcp3008.Channels.Single0).NormalizedValue;
				await this.UpdateWaterLevelUI(value);
			}
		}

		private void calibrateAppBarButton_Click(object sender, RoutedEventArgs e)
		{
			Frame rootFrame = Window.Current.Content as Frame;
			rootFrame.Navigate(typeof(CalibratePage), null);
			Window.Current.Activate();
		}
	}
}
