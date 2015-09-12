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
using Porrey.WaterLevel.Models;
using Windows.Devices.Sensors;
using Windows.Devices.Sensors.Calibration;
using Windows.Devices.Sensors.SensorStability;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Porrey.WaterLevel.Views
{
	public sealed partial class CalibratePage : BindablePage
	{
		private DispatcherTimer _timer = null;
		private Mcp3008 _mcp3008 = null;
		private MyApplicationSettings _applicationSettings = null;
		private CalibrationPoint[] _calibrationPoints = null;
		private int _calibrationIndex = 0;
		private readonly StabilityMonitor _stabilityMeter = new StabilityMonitor();

		private WizardStep[] _steps = null;

		public CalibratePage()
		{
			this.InitializeComponent();

			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);
			_timer.Tick += Timer_Tick;
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// ***
			// ***
			// ***
			_steps = new WizardStep[]
			{
				new WizardStep() { Instruction = "This wizard will walk you through the calibration steps. Follow the instructions carefully. Click Start to begin. You can press Cancel at any time.",
								   ButtonText = "Start",
								   StepAction = ()=> { } },

				new WizardStep() { Instruction = "Place the sensor in the water so that the water level is at 4 inches. Wait for the reading to stabilize and then click Next.",
								   ButtonText = "Next",
								   StepAction = async ()=> { await this.CaptureCalibrationPoint(4, this.SensorReading); this.OnPropertyChanged(nameof(CalibratrionPoint4)); } },

				new WizardStep() { Instruction = "Place the sensor in the water so that the water level is at 8 inches. Wait for the reading to stabilize and then click Next.",
								   ButtonText = "Next",
								   StepAction = async ()=> { await this.CaptureCalibrationPoint(8, this.SensorReading); this.OnPropertyChanged(nameof(CalibratrionPoint8)); } },

				new WizardStep() { Instruction = "Place the sensor in the water so that the water level is at 12 inches. Wait for the reading to stabilize and then click Next.",
								   ButtonText = "Next",
								   StepAction = async ()=> { await this.CaptureCalibrationPoint(12, this.SensorReading); this.OnPropertyChanged(nameof(CalibratrionPoint12)); } },

				new WizardStep() { Instruction = "Your sensor is now calibrated. Click Done to return the sensor reading.",
								   ButtonText = "Done",
								   StepAction = async ()=> { await this.SaveCalibrationPoints(); this.Return(); } }
			};

			// ***
			// *** Get the current calibration points
			// ***
			_calibrationPoints = this.ApplicationSettings.CalibrationPoints;

			// ***
			// *** Initialize the MCP3008
			// ***
			_mcp3008 = new Mcp3008(0);
			await _mcp3008.Initialize();

			// ***
			// *** Start the timer
			// ***
			_timer.Start();

			// ***
			// *** Start the wizard
			// ***
			await Start();
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

		#region Private Members
		private MyApplicationSettings ApplicationSettings
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

		private CalibrationPoint[] CalibrationPoints
		{
			get
			{
				return _calibrationPoints;
			}
			set
			{
				this._calibrationPoints = value;
			}
		}

		private void Return()
		{
			Frame rootFrame = Window.Current.Content as Frame;
			rootFrame.Navigate(typeof(MainPage), null);
			Window.Current.Activate();
		}

		private Task CaptureCalibrationPoint(int inches, float adjustedSensorReading)
		{
			// ***
			// *** Read the value from the sensor
			// ***
			this.CalibrationPoints[_calibrationIndex].X = adjustedSensorReading;
			this.CalibrationPoints[_calibrationIndex].Y = inches;

			// ***
			// *** Increment the counter
			// ***
			_calibrationIndex++;

			return Task.FromResult(0);
		}

		private Task SaveCalibrationPoints()
		{
			this.ApplicationSettings.CalibrationPoints = _calibrationPoints;

			return Task.FromResult(0);
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
				Mcp3008Reading xRef = _mcp3008.Read(Mcp3008.Channels.Single1);

				// ***
				// *** Get the normalized sensor reading
				// ***
				Mcp3008Reading x = _mcp3008.Read(Mcp3008.Channels.Single0);

				// ***
				// *** Adjust the sensor reading
				// ***
				float adjustedX = x.NormalizedValue / xRef.NormalizedValue;

				// ***
				// *** Update the stability meter
				// ***
				await _stabilityMeter.AddReading(adjustedX);

				// ***
				// *** Update the UI
				// ***
				await this.UpdateUI(xRef, x, adjustedX);
			}
		}

		private async Task UpdateUI(Mcp3008Reading reference, Mcp3008Reading reading, float adjustedReading)
		{
			if (this.Dispatcher != null)
			{
				await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					// ***
					// *** Update the sensor reading
					// ***
					this.SensorReading = adjustedReading;

					// ***
					// *** Update the dashboard values
					// ***
					this.RawSensorDisplayValue = reading.RawValue.ToString();
					this.RawReferenceDisplayValue = reference.RawValue.ToString();
					this.NormalizedSensorReadingDisplayValue = reading.NormalizedValue.ToString("0.000");
					this.AdjustedSensorReadingDisplayValue = adjustedReading.ToString("0.000");
					this.StabilityDisplayValue = string.Format("{0:0.000}", _stabilityMeter.Stability);
				});
			}
		}

		private float _sensorReading = 0f;
		private float SensorReading
		{
			get
			{
				return _sensorReading;
			}
			set
			{
				this.SetProperty(ref _sensorReading, value);
				this.OnPropertyChanged(nameof(SensorReadingDisplayValue));
			}
		}

		private Task Start()
		{
			this.StepNumber = 1;
			return Task.FromResult(0);
		}

		private Task NextStep()
		{
			// ***
			// *** Execute the previous step action
			// ***
			_steps[this.StepNumber - 1].StepAction();

			if (this.StepNumber < this.TotalSteps)
			{
				// ***
				// *** Go to the next step
				// ***
				this.StepNumber++;
			}

			return Task.FromResult(0);
		}
		#endregion

		#region Events
		private async void Timer_Tick(object sender, object e)
		{
			await ReadSensor();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.Return();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.NextStep();
		}

		private void ResetCalibration_Click(object sender, RoutedEventArgs e)
		{
			// ***
			// *** Reset the settings back to the default values
			// ***
			this.ApplicationSettings.ResetToDefaults();

			// ***
			// *** Get the current calibration points
			// ***
			_calibrationPoints = this.ApplicationSettings.CalibrationPoints;

			// ***
			// *** Update the UI
			// ***
			this.OnPropertyChanged(nameof(CalibratrionPoint4));
			this.OnPropertyChanged(nameof(CalibratrionPoint8));
			this.OnPropertyChanged(nameof(CalibratrionPoint12));
		}
		#endregion

		#region Bindings
		private int _stepNumber = 1;
		public int StepNumber
		{
			get
			{
				return _stepNumber;
			}
			set
			{
				this.SetProperty(ref _stepNumber, value);
				this.OnPropertyChanged(nameof(CurrentInstruction));
				this.OnPropertyChanged(nameof(ButtonText));
			}
		}

		public int TotalSteps
		{
			get
			{
				return _steps.Length;
			}
		}

		public string CurrentInstruction
		{
			get
			{
				return _steps[this.StepNumber - 1].Instruction;
			}
		}

		public string ButtonText
		{
			get
			{
				return _steps[this.StepNumber - 1].ButtonText;
			}
		}

		private bool _cancelEnabled = true;
		public bool CancelEnabled
		{
			get
			{
				return _cancelEnabled;
			}
			set
			{
				this.SetProperty(ref _cancelEnabled, value);
			}
		}

		public string SensorReadingDisplayValue
		{
			get
			{
				return string.Format("{0:0.00}", this.SensorReading);
			}
		}

		private string _rawSensorDisplayValue = string.Empty;
		public string RawSensorDisplayValue
		{
			get
			{
				return _rawSensorDisplayValue;
			}
			set
			{
				this.SetProperty(ref _rawSensorDisplayValue, value);
			}
		}

		private string _rawReferenceDisplayValue = string.Empty;
		public string RawReferenceDisplayValue
		{
			get
			{
				return _rawReferenceDisplayValue;
			}
			set
			{
				this.SetProperty(ref _rawReferenceDisplayValue, value);
			}
		}

		private string _normalizedSensorReadingDisplayValue = string.Empty;
		public string NormalizedSensorReadingDisplayValue
		{
			get
			{
				return _normalizedSensorReadingDisplayValue;
			}
			set
			{
				this.SetProperty(ref _normalizedSensorReadingDisplayValue, value);
			}
		}

		private string _adjustedSensorReadingDisplayValue = string.Empty;
		public string AdjustedSensorReadingDisplayValue
		{
			get
			{
				return _adjustedSensorReadingDisplayValue;
			}
			set
			{
				this.SetProperty(ref _adjustedSensorReadingDisplayValue, value);
			}
		}

		private string _stabilityDisplayValue = string.Empty;
		public string StabilityDisplayValue
		{
			get
			{
				return _stabilityDisplayValue;
			}
			set
			{
				this.SetProperty(ref _stabilityDisplayValue, value);
			}
		}

		public string CalibratrionPoint4
		{
			get
			{
				return this.CalibrationPoints[0].X.ToString("0.000");
			}
		}

		public string CalibratrionPoint8
		{
			get
			{
				return this.CalibrationPoints[1].X.ToString("0.000");
			}
		}

		public string CalibratrionPoint12
		{
			get
			{
				return this.CalibrationPoints[2].X.ToString("0.000");
			}
		}
		#endregion
	}
}
