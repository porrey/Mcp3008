// Copyright © 2015 Daniel Porrey
//
// This file is part of the MCP3008/Common solution.
// 
// MCP3008/Common is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// MCP3008/Common is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MCP3008/Common. If not, see http://www.gnu.org/licenses/.
//
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Porrey.Common
{
	public abstract class ApplicationSettings : BindableBase
	{		
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
			ApplicationData.Current.LocalSettings.Values.Clear();
			return Task.FromResult(0);
		}
	}
}
