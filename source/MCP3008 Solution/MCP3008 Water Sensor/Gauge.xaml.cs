// Copyright © 2015 Daniel Porrey
//
// This file is part of the MCP3008 Water Sensor solution.
// 
// MCP3008 Water Sensor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MCP3008 Water Sensor is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MCP3008 Water Sensor. If not, see http://www.gnu.org/licenses/.
//
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

