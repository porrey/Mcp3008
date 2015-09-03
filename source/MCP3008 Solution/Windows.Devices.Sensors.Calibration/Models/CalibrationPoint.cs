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
