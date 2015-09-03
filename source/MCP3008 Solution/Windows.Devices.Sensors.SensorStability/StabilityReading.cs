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
