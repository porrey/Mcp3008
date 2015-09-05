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
using System;

namespace Windows.Devices.Sensors
{
    public abstract class Mcp3008Exception : Exception
    {
		/// <summary>
		/// Initializes a new instance of the Windows.Devices.Sensors.Mcp3008Exception class.
		/// </summary>
		public Mcp3008Exception() : base()
        {
        }

		/// <summary>
		/// Initializes a new instance of the Windows.Devices.Sensors.Mcp3008Exception 
		/// class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public Mcp3008Exception(string message) : base(message)
        {
        }

		/// <summary>
		/// Initializes a new instance of the Windows.Devices.Sensors.Mcp3008Exception class with a 
		/// specified error message and a reference to the inner exception that is the 
		/// cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null 
		/// reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public Mcp3008Exception(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
