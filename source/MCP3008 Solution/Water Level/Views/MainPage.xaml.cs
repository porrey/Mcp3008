// Copyright © 2015 Daniel Porrey
//
// This file is part of the MCP3008/Water Level solution.
// 
// MCP3008/Water Level is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MCP3008/Water Level is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MCP3008/Water Level. If not, see http://www.gnu.org/licenses/.
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
		private AdjustedNonLinearCalibration _calibration = null;

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
			_calibration = new AdjustedNonLinearCalibration(MagicValue.Defaults.MinimumDepth, MagicValue.Defaults.MaximumDepth, this.ApplicationSettings.CalibrationPoints);

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

		private async Task ReadSensor()
		{
			if (_mcp3008 != null)
			{
				// ***
				// *** The reference voltage is measured from the reference resistor
				// *** on the sensor to help compensate for temperature changes. This
				// *** value is used to adjust the reading.
				// ***
				float xRef = _mcp3008.Read(Mcp3008.Channels.Single1).NormalizedValue;

				// ***
				// *** Get the normalized sensor reading
				// ***
				float x = _mcp3008.Read(Mcp3008.Channels.Single0).NormalizedValue;

				// ***
				// *** Adjust the sensor reading
				// ***
				float adjustedX = (x / xRef).Maximum(1f);

				// ***
				// *** Update the UI
				// ***
				await this.UpdateUI(adjustedX);
			}
		}

		private async Task UpdateUI(float x)
		{
			if (this.Dispatcher != null)
			{
				await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					// ***
					// *** Get the depth in inches
					// ***
					float y = _calibration.AdjustedReading(x).Minimum(0f).Maximum(MagicValue.Defaults.MaximumDepth);

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
			await this.ReadSensor();
		}

		private void calibrateAppBarButton_Click(object sender, RoutedEventArgs e)
		{
			Frame rootFrame = Window.Current.Content as Frame;
			rootFrame.Navigate(typeof(CalibratePage), null);
			Window.Current.Activate();
		}
	}
}
