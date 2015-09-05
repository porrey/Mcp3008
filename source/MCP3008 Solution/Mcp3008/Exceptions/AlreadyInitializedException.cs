// Copyright © 2015 Daniel Porrey
//
// This file is part of the Windows.Devices.Sensors.Mcp3008 project.
// 
// Windows.Devices.Sensors.Mcp3008 is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Windows.Devices.Sensors.Mcp3008 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Windows.Devices.Sensors.Mcp3008. If not, see http://www.gnu.org/licenses/.
//
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
