using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Windows.Devices.Sensors.SensorStability
{
	/// <summary>
	/// Measures the stability of multiple sensor readings over a period of time to determine
	/// if the reading is stable. This is useful, for example, for a sensor such as a scale where
	/// the weight will vary until while an object is placed on the scale but will eventually
	/// stabilize. This class aids in determining when such a sensor has a stable value.
	/// </summary>
	public class StabilityMeter
	{
		private double _minimumReading = 0;
		private double _maximumReading = 1;
		private int _sampleSize = 10;
		private bool _autoRange = true;
		private readonly List<StabilityReading<double>> _readings = new List<StabilityReading<double>>();

		/// <summary>
		/// Creates an instance of Windows.Devices.Sensors.SensorStability.StabilityMeter
		/// with the specified Maximum Consecutive readings required for a stable sensor 
		/// reading.
		/// </summary>
		/// <param name="minimumReading">Specifies the minimum or lowest value that 
		/// the sensor will output.</param>
		/// <param name="maximumReading">Specifies the maximum or highest value that
		/// the sensor will output.</param>		
		/// <param name="sampleSize">Specifies the number of samples to collect.</param>
		public StabilityMeter(double minimumReading, double maximumReading, int sampleSize = 10)
		{
			this.MaximumReading = maximumReading;
			this.SampleSize = sampleSize;
		}

		/// <summary>
		/// Gets/sets the minimum or lowest value that 
		/// the sensor will output.
		/// </summary>
		public double MinimumReading
		{
			get
			{
				return _minimumReading;
			}
			set
			{
				this._minimumReading = value;
			}
		}

		/// <summary>
		/// Gets/sets the maximum or highest value that
		/// the sensor will output.
		/// </summary>
		public double MaximumReading
		{
			get
			{
				return _maximumReading;
			}
			set
			{
				this._maximumReading = value;
			}
		}

		/// <summary>
		/// Gets/sets a value indicating if the minimum and maximum values
		/// should be adjusted as sensor readings are collected. True indicates that
		/// the minimum and maximum values will be adjusted; false indicates they 
		/// will remain static.
		/// </summary>
		public bool AutoRange
		{
			get
			{
				return _autoRange;
			}

			set
			{
				this._autoRange = value;
			}
		}

		/// <summary>
		/// Gets/sets the number of samples to collect to determine the
		/// stability of the sensor reading.
		/// </summary>
		public int SampleSize
		{
			get
			{
				return _sampleSize;
			}

			set
			{
				this._sampleSize = value;
			}
		}

		/// <summary>
		/// Gets a normalized value indicating the level of stability in the sensor reading.
		/// The value returned is a number between 0 and 1 where 0 is unstable and a value
		/// of 1 indicates perfect stability.
		/// </summary>
		public double Stability
		{
			get
			{
				// ***
				// *** Determine the Coefficient of Variance (normalized 
				// *** standard deviation)
				// ***
				double cv = this.NormalizedStandardDeviation / this.NormalizedAverage;
				return 1d - cv;
			}
		}

		/// <summary>
		/// Gets the last n consecutive readings where n is the
		/// MaximumConsecutiveReadings property.
		/// </summary>
		public List<StabilityReading<double>> Readings
		{
			get
			{
				return _readings;
			}
		}

		/// <summary>
		/// Adds a new reading to the meter and updates the stability measurement.
		/// </summary>
		/// <param name="sensorReading">The sensor reading of type T.</param>
		public Task AddReading(double sensorReading)
		{
			if (this.AutoRange)
			{
				// ***
				// *** Check the minimum reading
				// ***
				if (sensorReading < this.MinimumReading)
				{
					// ***
					// *** Adjust the minimum reading
					// ***
					this.MinimumReading = sensorReading;
				}

				// ***
				// *** Check the maximum reading
				// ***
				if (sensorReading > this.MaximumReading)
				{
					// ***
					// *** Adjust the maximum reading
					// ***
					this.MaximumReading = sensorReading;
				}
			}

			lock (this.Readings)
			{
				// ***
				// *** Add this value to the stack
				// ***
				this.Readings.Add(new StabilityReading<double>(DateTimeOffset.Now, sensorReading));

				// ***
				// *** Only keep the maximum number of readings in the list
				// ***
				if (this.Readings.Count > this.SampleSize)
				{
					this.Readings.Remove(Readings[0]);
				}
			}

			return Task.FromResult(0);
		}

		public double SensorRange
		{
			get
			{
				return this.MaximumReading - this.MinimumReading;
			}
		}

		public IEnumerable<double> NormalizedReadings
		{
			get
			{
				return this.Readings.Select(t => Math.Abs(this.MaximumReading - t.Value) / this.SensorRange);
			}
		}

		public double NormalizedAverage
		{
			get
			{
				// ***
				// *** Calculate the average
				// ***
				return this.NormalizedReadings.Average();
			}
		}

		public double NormalizedStandardDeviation
		{
			get
			{
				// ***
				// *** Calculate the standard deviation
				// ***
				double sum = this.NormalizedReadings.Sum(d => (d - this.NormalizedAverage) * (d - this.NormalizedAverage));
				return Math.Sqrt(sum / (double)this.Readings.Count());
			}
		}		
	}
}
