// Copyright © 2015 Daniel Porrey
//
// This file is part of the Windows.Devices.Sensors.Calibration project.
// 
// Windows.Devices.Sensors.Calibration is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Windows.Devices.Sensors.Calibration is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Windows.Devices.Sensors.Calibration. If not, see http://www.gnu.org/licenses/.
//
namespace Windows.Devices.Sensors.Calibration
{
    /// <summary>
    /// Represents an x- and y-coordinate pair in two-dimensional space.
    /// </summary>
    public struct CalibrationPoint
	{
		/// <summary>
		/// Gets or sets the X-coordinate value of 
		/// this Windows.Devices.Sensors.CalibrationPoint structure.
		/// </summary>
		public float X { get; set; }

		/// <summary>
		/// Gets or sets the Y-coordinate value of 
		/// this Windows.Devices.Sensors.CalibrationPoint structure.
		/// </summary>
		public float Y { get; set; }
	}
}
