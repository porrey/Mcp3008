// Copyright © 2015 Daniel Porrey
//
// This file is part of the MCP3008 Water Sensor solution.
// 
// MCP3008 Water Sensor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MCP3008 Water Sensor is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MCP3008 Water Sensor. If not, see http://www.gnu.org/licenses/.
//
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Porrey.WaterSensor.Common
{
	public class ApplicationSettings : BindableBase
	{
		#region Settings		
		public float CalibratedMaximumWaterSensorValue
		{
			get
			{
				return this.GetSetting<float>(MagicValue.Property.CalibratedMaximumWaterSensorValue, MagicValue.Defaults.CalibratedMaximumWaterSensorValue);
			}
			set
			{
				this.SaveSetting<float>(MagicValue.Property.CalibratedMaximumWaterSensorValue, value);
			}
		}
		#endregion

		public T GetSetting<T>(string name, T defaultValue)
		{
			T returnValue = default(T);

			if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(name))
			{
				// ***
				// *** Not all objects will serialize, so use Newtonsoft.Json
				// ***
				string json = (string)ApplicationData.Current.RoamingSettings.Values[name];
				returnValue = JsonConvert.DeserializeObject<T>(json);
			}
			else
			{
				returnValue = defaultValue;
			}

			return returnValue;
		}

		public void SaveSetting<T>(string name, T value)
		{
			// ***
			// *** Not all objects will serialize so use Newtonsoft.Json for everything
			// ***
			string json = JsonConvert.SerializeObject(value);
			ApplicationData.Current.RoamingSettings.Values[name] = json;
			this.OnPropertyChanged(name);
		}

		public Task ResetToDefaults()
		{
			ApplicationData.Current.RoamingSettings.Values.Clear();
			return Task.FromResult(0);
		}
	}
}
