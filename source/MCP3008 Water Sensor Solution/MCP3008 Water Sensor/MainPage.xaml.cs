using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Porrey.WaterSensor
{
	public sealed partial class MainPage : Page, INotifyPropertyChanged
	{
		private const float MaximumVoltage = 3.301f;
		private const int MaximumWaterSensorValue = 100;
		private const int TheoreticalMaximumWaterSensorValue = 56;
		private const int MaximumGuageValue = 360;

		private DispatcherTimer _timer = new DispatcherTimer();
		private CoreDispatcher _dispatcher = null;
		private Mcp3008 _mcp3008 = null;

		public MainPage()
		{
			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);
			_timer.Tick += Timer_Tick;
			_timer.Start();

			this.InitializeComponent();
		}

		public event PropertyChangedEventHandler PropertyChanged = null;

		private void OnPropertyChanged([CallerMemberName]string propertyName = null)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private string _voltageDisplayValue = "0.00";
		public string VoltageDisplayValue
		{
			get
			{
				return _voltageDisplayValue;
			}

			set
			{
				this._voltageDisplayValue = value;
				this.OnPropertyChanged();
			}
		}

		private double _voltageValue = 0;
		public double VoltageValue
		{
			get
			{
				return _voltageValue;
			}
			set
			{
				this._voltageValue = value;
				this.OnPropertyChanged();
			}
		}

		private string _waterSensorDisplayValue = "0.00";
		public string WaterSensorDisplayValue
		{
			get
			{
				return _waterSensorDisplayValue;
			}

			set
			{
				this._waterSensorDisplayValue = value;
				this.OnPropertyChanged();
			}
		}

		private double _waterSensorValue = 0;
		public double WaterSensorValue
		{
			get
			{
				return _waterSensorValue;
			}
			set
			{
				this._waterSensorValue = value;
				this.OnPropertyChanged();
			}
		}

		public string NoValue => string.Empty;

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			// ***
			// *** Get the current dispatcher for this view
			// ***
			_dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

			// ***
			// *** Initialize the MCP3008
			// ***
			_mcp3008 = new Mcp3008(0);
			await _mcp3008.Initialize();

			base.OnNavigatedTo(e);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
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
					this.VoltageValue = (int)(value / MaximumVoltage * (float)MaximumGuageValue);
				});
			}
		}

		private async Task UpdateWaterSensorUI(int value)
		{
			if (_dispatcher != null)
			{
				await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					// ***
					// *** The water sensor will not give the full voltage 3.3v)
					// *** when fully wet. So this will adjust it for a theoretical
					// *** maximum value.
					// ***
					int newValue = (int)(((float)value / (float)TheoreticalMaximumWaterSensorValue) * (float)MaximumWaterSensorValue);

					this.WaterSensorDisplayValue = newValue.ToString("0");
					this.WaterSensorValue = (newValue / 100f) * MaximumGuageValue;
				});
			}
		}

		private async void Timer_Tick(object sender, object e)
		{
			// ***
			// *** Update the water sensor (scale the value so it is a reading
			// *** between 0 and 100)
			// ***
			int waterSensor = _mcp3008.Read(Mcp3008.Channels.Differential0).AsRange(0, MaximumWaterSensorValue);
			await this.UpdateWaterSensorUI(waterSensor);

			// ***
			// *** Update the voltage (scale the value so it is a reading
			// *** between 0 and 3.301v)
			// ***
			float voltage = _mcp3008.Read(Mcp3008.Channels.Single0).AsScaledValue(MaximumVoltage);
			await this.UpdateVoltageUI(voltage);
		}
	}
}
