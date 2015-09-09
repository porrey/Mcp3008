// Copyright © 2015 Daniel Porrey
//
// This file is part of the MCP3008/Water Level solution.
// 
// MCP3008/Water Level is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MCP3008/Water Level is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MCP3008/Water Level. If not, see http://www.gnu.org/licenses/.
//
using System;

namespace Porrey.WaterLevel.Models
{
	/// <summary>
	/// Provides necessary properties for a wizard step in the calibration view
	/// </summary>
	internal class WizardStep
	{
		/// <summary>
		/// Gets/sets the instructions that are displayed to the user in the view
		/// </summary>
		public string Instruction { get; set; }

		/// <summary>
		/// Gets/sets the text that is displayed on the button
		/// </summary>
		public string ButtonText { get; set; }

		/// <summary>
		/// Gets/sets the action that is performed when the user clicks the button
		/// </summary>
		public Action StepAction { get; set; }
	}
}
