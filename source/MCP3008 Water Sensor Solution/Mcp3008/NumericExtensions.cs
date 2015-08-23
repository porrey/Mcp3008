namespace Windows.Devices.Sensors
{
	public static class NumericExtensions
	{
		/// <summary>
		/// Returns an adjusted value based on a calibration done
		/// to determine the maximum value of the device.
		/// </summary>
		/// <param name="value">The sensor reading to be calibrated.</param>
		/// <param name="calibratedMaximum">The maximum value based on calibration of the sensor.</param>
		/// <returns>Returns a float value adjusted based on the calibration value.</returns>
		public static float WithCalibration(this float value, float calibratedMaximum) => value / calibratedMaximum;

		/// <summary>
		/// Returns the value within the range specified.
		/// </summary>
		/// <param name="minimumValue">The lower end of the range and the minimum value returned.</param>
		/// <param name="maximumValue">The upper end of the range and the maximum value returned.</param>
		/// <returns>Returns a float value scaled value between the minimum and maximum value.</returns>
		public static float AsRange(this float value, float minimumValue, float maximumValue)
		{
			float returnValue = 0f;

			returnValue = (value * (maximumValue - minimumValue)) + minimumValue;

			if (returnValue < minimumValue)
			{
				returnValue = minimumValue;
			}
			else if (returnValue > maximumValue)
			{
				returnValue = maximumValue;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns the value within the range specified.
		/// </summary>
		/// <param name="minimumValue">The lower end of the range and the minimum value returned.</param>
		/// <param name="maximumValue">The upper end of the range and the maximum value returned.</param>
		/// <returns>Returns an integer value scaled value between the minimum and maximum value.</returns>
		public static int AsRange(this float value, int minimumValue, int maximumValue)
		{
			int returnValue = 0;

			returnValue = (int)((value * ((float)maximumValue - (float)minimumValue)) + (float)minimumValue);

			if (returnValue < minimumValue)
			{
				returnValue = minimumValue;
			}
			else if (returnValue > maximumValue)
			{
				returnValue = maximumValue;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns the value read from the channel as a scaled against
		/// the specified maximum value.
		/// </summary>
		/// <param name="maximumValue">The largest allowed value.</param>
		/// <returns>Returns a float value scaled value calculated as a percentage of the 
		/// maximum value using the normalized value.</returns>
		public static float AsScaledValue(this float value, float maximumValue) => value * maximumValue;

		/// <summary>
		/// Returns the value read from the channel as a scaled against
		/// the specified maximum value.
		/// </summary>
		/// <param name="maximumValue">The largest allowed value.</param>
		/// <returns>Returns an integer value scaled value calculated as a percentage of the 
		/// maximum value using the normalized value.</returns>
		public static int AsScaledValue(this float value, int maximumValue) => (int)(value * maximumValue);
	}
}
