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
using Windows.Devices.Sensors.Calibration;

namespace Porrey.WaterLevel.Common
{
	public static class MagicValue
	{
		public static class Property
		{
			public const string CalibrationPoints = "CalibrationPoints";
		}

		public static class Defaults
		{
			public const float MinimumDepth = 0f;
			public const float MaximumDepth = 12.5f;
			public static CalibrationPoint[] CalibrationPoints = new CalibrationPoint[4]
				{
					new CalibrationPoint() { Y = 4f, X = 0.9f },
					new CalibrationPoint() { Y = 8f, X = 0.8f },
					new CalibrationPoint() { Y = 12.25f, X = 0.6f },
                    new CalibrationPoint() { Y = 0f, X = 1f }
				};

			public const int MaximumGaugeValue = 360;
			public const string WaterLevelDisplay = "0.00 in";
			public const int WaterLevelValue = 0;
		}

		public static class Format
		{
			public const string WaterLevelFormat = "0.00 in";
		}
	}
}