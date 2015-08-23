using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Porrey.WaterSensor
{
	public sealed partial class Gauge : UserControl
	{
		public Gauge()
		{
			this.InitializeComponent();
		}

		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(Gauge), new PropertyMetadata(0d));
		
		public static readonly DependencyProperty DisplayStringProperty = DependencyProperty.Register("DisplayString", typeof(string), typeof(Gauge), new PropertyMetadata("0"));

		public double Value
		{
			get
			{
				return (double)GetValue(ValueProperty);
			}
			set
			{
				SetValue(ValueProperty, value);
			}
		}

		public string DisplayString
		{
			get
			{
				return (string)GetValue(DisplayStringProperty);
			}
			set
			{
				SetValue(DisplayStringProperty, value);
			}
		}
	}
}

