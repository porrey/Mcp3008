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
using Porrey.WaterSensor.Common;
using Windows.Devices.Sensors;
using Windows.Devices.Sensors.Calibration;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Porrey.WaterSensor.Views
{
	public sealed partial class MainPage : BindablePage
	{
		private DispatcherTimer _timer = new DispatcherTimer();
		private CoreDispatcher _dispatcher = null;
		private Mcp3008 _mcp3008 = null;
		private float _calibratedMaximumWaterSensorValue = MagicValue.Defaults.CalibratedMaximumWaterSensorValue;
		private MyApplicationSettings _applicationSettings = null;

		public MainPage()
		{
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);
			_timer.Tick += Timer_Tick;

			this.InitializeComponent();
		}

		private string _voltageDisplayValue = MagicValue.Defaults.VoltageDisplay;
		public string VoltageDisplayValue
		{
			get
			{
				return _voltageDisplayValue;
			}

			set
			{
				this.SetProperty(ref _voltageDisplayValue, value);
			}
		}

		private double _voltageValue = MagicValue.Defaults.VoltageValue;
		public double VoltageValue
		{
			get
			{
				return _voltageValue;
			}
			set
			{
				this.SetProperty(ref _voltageValue, value);
			}
		}

		private string _waterSensorDisplayValue = MagicValue.Defaults.WaterSensorDisplay;
		public string WaterSensorDisplayValue
		{
			get
			{
				return _waterSensorDisplayValue;
			}

			set
			{
				this.SetProperty(ref _waterSensorDisplayValue, value);
			}
		}

		private double _waterSensorValue = MagicValue.Defaults.WaterSensorValue;
		public double WaterSensorValue
		{
			get
			{
				return _waterSensorValue;
			}
			set
			{
				this.SetProperty(ref _waterSensorValue, value);
			}
		}

		private bool _calibrationIsActive = false;
		public bool CalibrationIsActive
		{
			get
			{
				return _calibrationIsActive;
			}
			set
			{
				this.SetProperty(ref _calibrationIsActive, value);
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
			// *** Get the current dispatcher for this view
			// ***
			_dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

			// ***
			// *** Get the calibration value from application settings
			// ***
			_calibratedMaximumWaterSensorValue = this.ApplicationSettings.CalibratedMaximumWaterSensorValue;

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

			_dispatcher = null;

			base.OnNavigatedFrom(e);
		}

		private async Task UpdateVoltageUI(float value)
		{
			if (_dispatcher != null)
			{
				await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					this.VoltageDisplayValue = value.ToString("0.000v");

					// ***
					// *** The gauge wants a value between 0 and 360
					// ***
					this.VoltageValue = (int)(value / MagicValue.Defaults.MaximumVoltage * (float)MagicValue.Defaults.MaximumGaugeValue);
				});
			}
		}

		private async Task UpdateWaterSensorUI(float value)
		{
			if (_dispatcher != null)
			{
				await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					this.WaterSensorDisplayValue = value.ToString("0%");

					// ***
					// *** The gauge wants a value between 0 and 360
					// ***
					this.WaterSensorValue = value.AsRange(0, MagicValue.Defaults.MaximumGaugeValue);
				});
			}
		}

		private async void Timer_Tick(object sender, object e)
		{
			if (!this.CalibrationIsActive)
			{
				// ***
				// *** The water sensor will not give the full voltage 3.3v)
				// *** when fully wet. So this will adjust it for a theoretical
				// *** maximum value and return an int between 0 and 1.
				// ***
				float waterSensor = _mcp3008.Read(Mcp3008.Channels.Single1).NormalizedValue.WithCalibration(_calibratedMaximumWaterSensorValue).AsRange(0f, 1f);
				await this.UpdateWaterSensorUI(waterSensor);
			}

			// ***
			// *** Update the voltage (scale the value so it is a reading
			// *** between 0 and 3.301v)
			// ***
			float voltage = _mcp3008.Read(Mcp3008.Channels.Single0).NormalizedValue.AsScaledValue(MagicValue.Defaults.MaximumVoltage);
			await this.UpdateVoltageUI(voltage);
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// ***
				// *** Disable the button
				// ***
				this.CalibrationIsActive = true;

				// ***
				// *** Get the next ten readings about 500ms apart
				// ***
				float totalValue = 0f;

				for (int i = 0; i < 10; i++)
				{
					// ***
					// *** Read the value and add it to the total
					// ***
					totalValue += _mcp3008.Read(Mcp3008.Channels.Single1).NormalizedValue;

					// ***
					// *** Wait 500 ms
					// ***
					await Task.Delay(500);
				}

				// ***
				// *** Calculate and set the calibration value
				// ***
				_calibratedMaximumWaterSensorValue = totalValue / 10f;

				// ***
				// *** Save the calibration value to application settings
				// ***
				this.ApplicationSettings.CalibratedMaximumWaterSensorValue = _calibratedMaximumWaterSensorValue;
			}
			finally
			{
				this.CalibrationIsActive = false;
			}
		}
	}
}
