using System;

namespace Windows.Devices.Sensors.Calibration
{
	/// <summary>
	/// Allows calibration of a device using a 2nd Order Polynomial
	/// in the form y = ax² + bx + c.
	/// </summary>
	public class NonLinearCalibration : Calibration
	{
		/// <summary>
		/// The number of calibration points required by this calibration method.
		/// </summary>
		private const int _calibrationPointCount = 3;

		/// <summary>
		/// Represents the value a in the formula y = ax² + bx + c
		/// </summary>
		private float _a = 0f;

		/// <summary>
		/// Represents the value b in the formula y = ax² + bx + c
		/// </summary>
		private float _b = 0f;

		/// <summary>
		/// Represents the value c in the formula y = ax² + bx + c
		/// </summary>
		private float _c = 0f;

		/// <summary>
		/// Creates an instance of Windows.Devices.Sensors.NonLinearCalibration
		/// with the specified point count and maximum reading value.
		/// </summary>
		/// <param name="maximum">The maximum adjusted reading value allowed.</param>
		public NonLinearCalibration(float maximum) 
			: base(_calibrationPointCount, maximum)
		{
		}

		/// <summary>
		/// Creates an instance of Windows.Devices.Sensors.NonLinearCalibration
		/// with the specified point count and minimum and maximum 
		/// reading values. 
		/// </summary>
		/// <param name="minimum">The minimum adjusted reading value allowed.</param>
		/// <param name="maximum">The maximum adjusted reading value allowed.</param>
		public NonLinearCalibration(float minimum, float maximum) 
			: base(_calibrationPointCount, minimum, maximum)
		{
		}

		/// <summary>
		/// Creates an instance of Windows.Devices.Sensors.NonLinearCalibration
		/// with the specified point count, maximum 
		/// reading values and calibration points.
		/// </summary>
		/// <param name="maximum">The maximum adjusted reading value allowed.</param>
		/// <param name="calibrationPoints">The calibrations points used to adjust the reading.</param>
		public NonLinearCalibration(float maximum, CalibrationPoint[] calibrationPoints)
			: base(_calibrationPointCount, maximum, calibrationPoints)
        {
		}

		/// <summary>
		/// Creates an instance of Windows.Devices.Sensors.NonLinearCalibration
		/// with the specified point count, minimum and maximum 
		/// reading values and calibration points.
		/// </summary>
		/// <param name="minimum">The minimum adjusted reading value allowed.</param>
		/// <param name="maximum">The maximum adjusted reading value allowed.</param>
		/// <param name="calibrationPoints">The calibrations points used to adjust the reading.</param>
		public NonLinearCalibration(float minimum, float maximum, CalibrationPoint[] calibrationPoints)
			: base(_calibrationPointCount, minimum, maximum, calibrationPoints)
		{
		}

		protected override float OnAdjustedReading(float x)
		{
			return (x * x * _a) + (_b * x) + _c;
		}

		protected override void OnCalibrationPointsChanged(CalibrationPoint[] calibrationPoints)
		{
			this.CalculateFormulaVariables(calibrationPoints, out _a, out _b, out _c);
		}

		/// <summary>
		/// Calculates the values a, b and c in the formula y = ax² + bx + c
		/// </summary>
		protected virtual void CalculateFormulaVariables(CalibrationPoint[] calibrationPoints, out float a, out float b, out float c)
		{
			if (calibrationPoints.Length == this.CalibrationPointCount)
			{
				// ***
				// *** These value are cast into variables here to hopefully
				// *** makes this more readable
				// ***
				float x1 = calibrationPoints[0].X;
				float x2 = calibrationPoints[1].X;
				float x3 = calibrationPoints[2].X;

				float y1 = calibrationPoints[0].Y;
				float y2 = calibrationPoints[1].Y;
				float y3 = calibrationPoints[2].Y;

				float denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
				a = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
				b = (x3 * x3 * (y1 - y2) + x2 * x2 * (y3 - y1) + x1 * x1 * (y2 - y3)) / denom;
				c = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;
			}
			else if (calibrationPoints.Length < this.CalibrationPointCount)
			{
				// ***
				// *** Throw a specific exception letting the caller 
				// *** know there are less than the three required points.
				// ***
				throw new ArgumentOutOfRangeException(string.Format("There are too few points defined. {0} must be an array of three points.", nameof(calibrationPoints)));
			}
			else
			{
				// ***
				// *** Throw a specific exception letting the caller 
				// *** know there are more than the three required points.
				// ***
				throw new ArgumentOutOfRangeException(string.Format("There are too many points defined. {0} must be an array of three points.", nameof(calibrationPoints)));
			}
		}

		public void Calc(float x1, float y1, float x2, float y2, float x3, float y3)
		{
			float denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
			float A = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
			float B = (x3 * x3 * (y1 - y2) + x2 * x2 * (y3 - y1) + x1 * x1 * (y2 - y3)) / denom;
			float C = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;
		}
	}
}
