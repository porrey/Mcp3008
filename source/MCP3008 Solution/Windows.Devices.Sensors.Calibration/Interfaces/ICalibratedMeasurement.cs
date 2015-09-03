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
