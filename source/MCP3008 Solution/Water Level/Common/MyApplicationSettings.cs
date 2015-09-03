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
using Porrey.Common;
using Windows.Devices.Sensors.Calibration;

namespace Porrey.WaterLevel.Common
{
	public class MyApplicationSettings : ApplicationSettings
	{
		public CalibrationPoint[] CalibrationPoints
		{
			get
			{
				return this.GetSetting<CalibrationPoint[]>(MagicValue.Property.CalibrationPoints, MagicValue.Defaults.CalibrationPoints);
			}
			set
			{
				this.SaveSetting<CalibrationPoint[]>(MagicValue.Property.CalibrationPoints, value);
			}
		}
	}
}
