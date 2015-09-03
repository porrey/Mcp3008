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
namespace Porrey.WaterSensor.Common
{
	public static class MagicValue
	{
		public static class Property
		{
			public const string CalibratedMaximumWaterSensorValue = "CalibratedMaximumWaterSensorValue";
        }

		public static class Defaults
		{
			public const float CalibratedMaximumWaterSensorValue = .57f;
			public const float MaximumVoltage = 3.301f;
			public const int MaximumWaterSensorValue = 100;
			public const int MaximumGaugeValue = 360;
			public const string VoltageDisplay = "0.00";
			public const float VoltageValue = 0f;
			public const string WaterSensorDisplay = "0%";
			public const float WaterSensorValue = 0f;
		}
	}
}