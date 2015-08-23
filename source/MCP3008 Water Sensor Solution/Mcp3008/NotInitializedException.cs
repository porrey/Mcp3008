namespace Windows.Devices.Sensors
{
	/// <summary>
	/// Indicates that the MCP3008 has not been initialized or it was
	/// disposed after being initialized.
	/// </summary>
	public sealed class NotInitializedException : Mcp3008Exception
	{
		/// <summary>
		/// Initializes a new instance of the Windows.Devices.Sensors.NotInitializedException class.
		/// </summary>
		public NotInitializedException()
			: base("The MCP3008 has not been initialized or it has been disposed.")
		{
		}
	}
}
