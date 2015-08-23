namespace Windows.Devices.Sensors
{
	/// <summary>
	/// Indicates that the MCP3008 class has already been initialized
	/// and cannot be initialized again.
	/// </summary>
	public sealed class AlreadyInitializedException : Mcp3008Exception
    {
		/// <summary>
		/// Initializes a new instance of the Windows.Devices.Sensors.AlreadyInitializedException class.
		/// </summary>
		public AlreadyInitializedException()
            : base("The MCP3008 has already been initialized.")
        {
        }
    }
}
