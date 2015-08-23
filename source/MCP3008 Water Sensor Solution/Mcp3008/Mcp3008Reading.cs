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
