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
	public interface ICalibratedMeasurement
	{
		/// <summary>
		/// Gets sets the maximum adjusted reading.
		/// </summary>
		float Maximum { get; set; }

		/// <summary>
		/// Gets sets the minimum adjusted reading.
		/// </summary>
		float Minimum { get; set; }

		/// <summary>
		/// Gets the three calibration point used to calibrate
		/// the device measurement.
		/// </summary>
		CalibrationPoint[] CalibrationPoints { get; set; }

		/// <summary>
		/// Returns the current adjusted reading the device. The 
		/// reading is a value between 0 and 1.
		/// </summary>
		/// <returns>Returns the adjusted sensor reading.</returns>
		float AdjustedReading(float sensorReading);

		/// <summary>
		/// Gets the number of calibration points expected for this 
		/// implementation of ICalibratedMeasurement.
		/// </summary>
		int CalibrationPointCount { get; }
	}
}
