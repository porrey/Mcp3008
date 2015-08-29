namespace Windows.Devices.Sensors
{
	/// <summary>
	/// 
	/// </summary>
	public enum InputConfiguration
	{
		/// <summary>
		/// 
		/// </summary>
		SingleEnded = 1,
		/// <summary>
		/// 
		/// </summary>
		Differential = 0
	}

	public class Channel
	{
		internal Channel(InputConfiguration selection, int id)
		{
			this.Id = id;
			this.InputConfiguration = selection;
		}

		public int Id { get; set; }
		public InputConfiguration InputConfiguration { get; set; }
	}
}
