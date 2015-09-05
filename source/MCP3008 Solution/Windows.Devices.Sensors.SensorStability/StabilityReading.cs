// Copyright © 2015 Daniel Porrey
//
// This file is part of the Windows.Devices.Sensors.SensorStability project.
// 
// Windows.Devices.Sensors.SensorStability is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Windows.Devices.Sensors.SensorStability is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Windows.Devices.Sensors.SensorStability. If not, 
// see http://www.gnu.org/licenses/.
//
using System;

namespace Windows.Devices.Sensors.SensorStability
{
	/// <summary>
	/// Contains a sensor reading with a date/time stamp.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class StabilityReading<T>
	{
		internal StabilityReading(DateTimeOffset dateTimeStamp, T value)
		{
			this.DateTimeStamp = dateTimeStamp;
			this.Value = value;
		}

		/// <summary>
		/// Gets/sets the date/time stamp fo the sensor reading.
		/// </summary>
		public DateTimeOffset DateTimeStamp { get; set; }

		/// <summary>
		/// Gets/sets the sensor reading value.
		/// </summary>
		public T Value { get; set; }

		public override string ToString()
		{
			return string.Format("{0} [{1}]", this.Value, this.DateTimeStamp);
		}
	}
}
