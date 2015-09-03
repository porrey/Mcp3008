using System;
using System.Threading.Tasks;
using Porrey.Common;
using Porrey.WaterLevel.Common;
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
		private NonLinearCalibration _calibration = null;
		private CalibrationPoint[] _calibrationPoints = new CalibrationPoint[3];
		private int _calibrationIndex = 0;
		private readonly StabilityMeter _stabilityMeter = new StabilityMeter(.35d, .57d);

		public CalibratePage()
		{
			this.InitializeComponent();

			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);
			_timer.Tick += Timer_Tick;
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

		private bool _okEnabled = false;
		public bool OkEnabled
		{
			get
			{
				return _okEnabled;
			}
			set
			{
				this.SetProperty(ref _okEnabled, value);
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

		private bool _startEnabled = true;
		public bool StartEnabled
		{
			get
			{
				return _startEnabled;
			}
			set
			{
				this.SetProperty(ref _startEnabled, value);
			}
		}

		private bool _zeroEnabled = false;
		public bool ZeroEnabled
		{
			get
			{
				return _zeroEnabled;
			}
			set
			{
				this.SetProperty(ref _zeroEnabled, value);
			}
		}

		private bool _oneEnabled = false;
		public bool OneEnabled
		{
			get
			{
				return _oneEnabled;
			}
			set
			{
				this.SetProperty(ref _oneEnabled, value);
			}
		}

		private bool _twoEnabled = false;
		public bool TwoEnabled
		{
			get
			{
				return _twoEnabled;
			}
			set
			{
				this.SetProperty(ref _twoEnabled, value);
			}
		}

		private bool _threeEnabled = false;
		public bool ThreeEnabled
		{
			get
			{
				return _threeEnabled;
			}
			set
			{
				this.SetProperty(ref _threeEnabled, value);
			}
		}

		private bool _fourEnabled = false;
		public bool FourEnabled
		{
			get
			{
				return _fourEnabled;
			}
			set
			{
				this.SetProperty(ref _fourEnabled, value);
			}
		}

		private bool _fiveeEnabled = false;
		public bool FiveEnabled
		{
			get
			{
				return _fiveeEnabled;
			}
			set
			{
				this.SetProperty(ref _fiveeEnabled, value);
			}
		}

		private bool _sixEnabled = false;
		public bool SixEnabled
		{
			get
			{
				return _sixEnabled;
			}
			set
			{
				this.SetProperty(ref _sixEnabled, value);
			}
		}

		private bool _sevenEnabled = false;
		public bool SevenEnabled
		{
			get
			{
				return _sevenEnabled;
			}
			set
			{
				this.SetProperty(ref _sevenEnabled, value);
			}
		}

		private bool _eightEnabled = false;
		public bool EightEnabled
		{
			get
			{
				return _eightEnabled;
			}
			set
			{
				this.SetProperty(ref _eightEnabled, value);
			}
		}

		private bool _nineEnabled = false;
		public bool NineEnabled
		{
			get
			{
				return _nineEnabled;
			}
			set
			{
				this.SetProperty(ref _nineEnabled, value);
			}
		}

		private bool _tenEnabled = false;
		public bool TenEnabled
		{
			get
			{
				return _tenEnabled;
			}
			set
			{
				this.SetProperty(ref _tenEnabled, value);
			}
		}

		private bool _elevenEnabled = false;
		public bool ElevenEnabled
		{
			get
			{
				return _elevenEnabled;
			}
			set
			{
				this.SetProperty(ref _elevenEnabled, value);
			}
		}

		private bool _twelveEnabled = false;
		public bool TwelveEnabled
		{
			get
			{
				return _twelveEnabled;
			}
			set
			{
				this.SetProperty(ref _twelveEnabled, value);
			}
		}

		public CalibrationPoint[] CalibrationPoints
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

		private void Ok_Click(object sender, RoutedEventArgs e)
		{
			this.SaveCalibrationPoints();
			this.Return();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.Return();
		}

		private void Start_Click(object sender, RoutedEventArgs e)
		{
			// ***
			// *** Enable all of the buttons
			// ***
			this.ZeroEnabled = true;
			this.OneEnabled = true;
			this.TwoEnabled = true;
			this.ThreeEnabled = true;
			this.FourEnabled = true;
			this.FiveEnabled = true;
			this.SixEnabled = true;
			this.SevenEnabled = true;
			this.EightEnabled = true;
			this.NineEnabled = true;
			this.TenEnabled = true;
			this.ElevenEnabled = true;
			this.TwelveEnabled = true;
		}

		private async void Zero_Click(object sender, RoutedEventArgs e)
		{
			this.ZeroEnabled = false;
			await this.GetCalibrationPoint(0);
		}

		private async void One_Click(object sender, RoutedEventArgs e)
		{
			this.OneEnabled = false;
			await this.GetCalibrationPoint(1);
		}

		private async void Two_Click(object sender, RoutedEventArgs e)
		{
			this.TwoEnabled = false;
			await this.GetCalibrationPoint(2);
		}

		private async void Three_Click(object sender, RoutedEventArgs e)
		{
			this.ThreeEnabled = false;
			await this.GetCalibrationPoint(3);
		}

		private async void Four_Click(object sender, RoutedEventArgs e)
		{
			this.FourEnabled = false;
			await this.GetCalibrationPoint(4);
		}

		private async void Five_Click(object sender, RoutedEventArgs e)
		{
			this.FiveEnabled = false;
			await this.GetCalibrationPoint(5);
		}

		private async void Six_Click(object sender, RoutedEventArgs e)
		{
			this.SixEnabled = false;
			await this.GetCalibrationPoint(6);
		}

		private async void Seven_Click(object sender, RoutedEventArgs e)
		{
			this.SevenEnabled = false;
			await this.GetCalibrationPoint(7);
		}

		private async void Eight_Click(object sender, RoutedEventArgs e)
		{
			this.EightEnabled = false;
			await this.GetCalibrationPoint(8);
		}

		private async void Nine_Click(object sender, RoutedEventArgs e)
		{
			this.NineEnabled = false;
			await this.GetCalibrationPoint(9);
		}

		private async void Ten_Click(object sender, RoutedEventArgs e)
		{
			this.TenEnabled = false;
			await this.GetCalibrationPoint(10);
		}

		private async void Eleven_Click(object sender, RoutedEventArgs e)
		{
			this.ElevenEnabled = false;
			await this.GetCalibrationPoint(11);
		}

		private async void Twelve_Click(object sender, RoutedEventArgs e)
		{
			this.TwelveEnabled = false;
			await this.GetCalibrationPoint(12);
		}

		private void Return()
		{
			Frame rootFrame = Window.Current.Content as Frame;
			rootFrame.Navigate(typeof(MainPage), null);
			Window.Current.Activate();
		}

		private Task GetCalibrationPoint(int index)
		{
			// ***
			// *** Read the value from the sensor
			// ***
			this.CalibrationPoints[_calibrationIndex].Y = _mcp3008.Read(Mcp3008.Channels.Single0).NormalizedValue;
			this.CalibrationPoints[_calibrationIndex].X = index;

			// ***
			// *** Increment the counter
			// ***
			_calibrationIndex++;

			if (_calibrationIndex == 3)
			{
				// ***
				// *** Update the OK button
				// ***
				this.OkEnabled = true;
			}

			return Task.FromResult(0);
		}

		private Task SaveCalibrationPoints()
		{
			this.ApplicationSettings.CalibrationPoints = _calibrationPoints;

			return Task.FromResult(0);
		}

		private float _sensorReading = 0f;
		public float SensorReading
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

		public string SensorReadingDisplayValue
		{
			get
			{
				return string.Format("{0:0.00}", this.SensorReading);
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
				float x = _mcp3008.Read(Mcp3008.Channels.Single0).NormalizedValue;

				// ***
				// *** Update the stability meter
				// ***
				await _stabilityMeter.AddReading(x);

				if (this.Dispatcher != null)
				{
					await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
					{
						// ***
						// *** Update the display
						// ***
						this.SensorReading = x;

						// ***
						// *** Update the stability meter
						// ***
						if (_stabilityMeter.Stability < .85)
						{
							this.Ready = Visibility.Collapsed;
							this.Stabilizing = Visibility.Collapsed;
							this.NotReady = Visibility.Visible;
						}
						else if (_stabilityMeter.Stability <= .95)
						{
							this.Ready = Visibility.Collapsed;
							this.Stabilizing = Visibility.Visible;
							this.NotReady = Visibility.Collapsed;
						}
						else
						{
							this.Ready = Visibility.Visible;
							this.Stabilizing = Visibility.Collapsed;
							this.NotReady = Visibility.Collapsed;
						}
					});
				}
			}
		}

		private Visibility _notReady = Visibility.Collapsed;
		public Visibility NotReady
		{
			get
			{
				return _notReady;
			}
			set
			{
				this.SetProperty(ref _notReady, value);
			}
		}

		private Visibility _stabilizing = Visibility.Collapsed;
		public Visibility Stabilizing
		{
			get
			{
				return _stabilizing;
			}
			set
			{
				this.SetProperty(ref _stabilizing, value);
			}
		}

		private Visibility _ready = Visibility.Visible;
		public Visibility Ready
		{
			get
			{
				return _ready;
			}
			set
			{
				this.SetProperty(ref _ready, value);
			}
		}
	}
}
