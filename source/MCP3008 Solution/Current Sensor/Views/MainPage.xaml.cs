// Copyright © 2015 Daniel Porrey
//
// This file is part of the MCP3008/Current Sensor solution.
// 
// MCP3008/Current Sensor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MCP3008/Current Sensor is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MCP3008/Current Sensor. If not, see http://www.gnu.org/licenses/.
//
using System;
using System.Threading.Tasks;
using Porrey.Common;
using Porrey.CurrentSensor.Common;
using Windows.Devices.Sensors;
using Windows.Devices.Sensors.Calibration;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Porrey.CurrentSensor.Views
{
	public sealed partial class MainPage : BindablePage
	{
		private DispatcherTimer _timer = null;
		private Mcp3008 _mcp3008 = null;
		private MyApplicationSettings _applicationSettings = null;

		public MainPage()
		{
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);
			_timer.Tick += Timer_Tick;

			this.InitializeComponent();
		}

		private string _currentDisplayValue = "0";
		public string CurrentDisplayValue
		{
			get
			{
				return _currentDisplayValue;
			}

			set
			{
				this.SetProperty(ref _currentDisplayValue, value);
			}
		}

		private float _currentValue = 0;
		public float CurrentValue
		{
			get
			{
				return _currentValue;
			}
			set
			{
				this.SetProperty(ref _currentValue, value);
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
				// ***
				// ***
				float current = _mcp3008.Read(Mcp3008.Channels.Single2).NormalizedValue * 3.301f;

				// ***
				// *** Update the UI
				// ***
				await this.UpdateUI(current);
			}
		}

		private async Task UpdateUI(float x)
		{
			if (this.Dispatcher != null)
			{
				await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					// ***
					// *** Convert to a string for display
					// ***
					this.CurrentDisplayValue = x.ToString("0.000000");

					// ***
					// *** The gauge wants a value between 0 and 360
					// ***
					this.CurrentValue = x.AsRange(0, MagicValue.Defaults.MaximumGaugeValue);
				});
			}
		}

		private async void Timer_Tick(object sender, object e)
		{
			await this.ReadSensor();
		}
	}
}
