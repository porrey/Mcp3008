namespace Windows.Devices.Sensors
{
	/// <summary>
	/// Contains the values read from a channel on the MCP3008.
	/// </summary>
	public partial class Mcp3008Reading
	{
		/// <summary>
		/// Creates a default instance of Windows.Devices.Sensors.Mcp3008Reading.
		/// </summary>
		internal Mcp3008Reading()
		{
		}

		/// <summary>
		/// Creates a default instance of Windows.Devices.Sensors.Mcp3008Reading
		/// with the given raw value.
		/// </summary>
		internal Mcp3008Reading(int rawValue)
		{
			this.RawValue = rawValue;
		}

		/// <summary>
		/// Gets the actual value read from the channel.
		/// </summary>
		public int RawValue { get; set; }

		/// <summary>
		/// Gets a normalized value in the range of 0 to 1.
		/// </summary>
		public float NormalizedValue => this.RawValue / 1024f;

		/// <summary>
		/// Returns the value read from the channel as a scaled against
		/// the specified maximum value.
		/// </summary>
		/// <param name="maximumValue">The largest allowed value.</param>
		/// <returns>Returns a float value scaled value calculated as a percentage of the 
		/// maximum value using the normalized value.</returns>
		public float AsScaledValue(float maximumValue) => this.NormalizedValue * maximumValue;

		/// <summary>
		/// Returns the value read from the channel as a scaled against
		/// the specified maximum value.
		/// </summary>
		/// <param name="maximumValue">The largest allowed value.</param>
		/// <returns>Returns an integer value scaled value calculated as a percentage of the 
		/// maximum value using the normalized value.</returns>
		public int AsScaledValue(int maximumValue) => (int)(this.NormalizedValue * maximumValue);

		/// <summary>
		/// Returns the value within the range specified.
		/// </summary>
		/// <param name="minimumValue">The lower end of the range and the minimum value returned.</param>
		/// <param name="maximumValue">The upper end of the range and the maximum value returned.</param>
		/// <returns>Returns a float value scaled value between the minimum and maximum value.</returns>
		public float AsRange(float minimumValue, float maximumValue) => (this.NormalizedValue * (maximumValue - minimumValue)) + minimumValue;

		/// <summary>
		/// Returns the value within the range specified.
		/// </summary>
		/// <param name="minimumValue">The lower end of the range and the minimum value returned.</param>
		/// <param name="maximumValue">The upper end of the range and the maximum value returned.</param>
		/// <returns>Returns an integer value scaled value between the minimum and maximum value.</returns>
		public int AsRange(int minimumValue, int maximumValue) => (int)((this.NormalizedValue * ((float)maximumValue - (float)minimumValue)) + (float)minimumValue);

		/// <summary>
		/// Implicitly converts the value read from the channel to
		/// a normalized float value.
		/// </summary>
		/// <param name="value">Returns the normalized value.</param>
		public static implicit operator float (Mcp3008Reading value) => value.NormalizedValue;

		/// <summary>
		/// Implicitly converts the value read from the channel to
		/// the raw integer value.
		/// </summary>
		/// <param name="value">Returns the raw value.</param>
		public static implicit operator int (Mcp3008Reading value) => value.RawValue;
	}
}
