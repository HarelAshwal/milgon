using System;
using System.Diagnostics;

internal sealed class SunTimes
{
	private const double mDR = 0.0174532925199433;

	private const double mK1 = 0.262516168343005;

	private object mLock = new object();

	private int[] mRiseTimeArr = new int[2];

	private int[] mSetTimeArr = new int[2];

	private double mRizeAzimuth = 0;

	private double mSetAzimuth = 0;

	private double[] mSunPositionInSkyArr = new double[2];

	private double[] mRightAscentionArr = new double[3];

	private double[] mDecensionArr = new double[3];

	private double[] mVHzArr = new double[3];

	private bool mIsSunrise = false;

	private bool mIsSunset = false;

	private readonly static SunTimes mInstance;

	public static SunTimes Instance
	{
		get
		{
			return SunTimes.mInstance;
		}
	}

	static SunTimes()
	{
		SunTimes.mInstance = new SunTimes();
	}

	private SunTimes()
	{
	}

	private void CalculateSunPosition(double jd, double ct)
	{
		double num = 0.779072 + 0.00273790931 * jd;
		num -= Math.Floor(num);
		num = num * 2 * 3.14159265358979;
		double num1 = 0.993126 + 0.0027377785 * jd;
		num1 -= Math.Floor(num1);
		num1 = num1 * 2 * 3.14159265358979;
		double num2 = 0.39785 * Math.Sin(num);
		num2 = num2 - 0.01 * Math.Sin(num - num1);
		num2 = num2 + 0.00333 * Math.Sin(num + num1);
		num2 = num2 - 0.00021 * ct * Math.Sin(num);
		double num3 = 1 - 0.03349 * Math.Cos(num1);
		num3 = num3 - 0.00014 * Math.Cos(2 * num);
		num3 = num3 + 8E-05 * Math.Cos(num);
		double num4 = -0.0001 - 0.04129 * Math.Sin(2 * num);
		num4 = num4 + 0.03211 * Math.Sin(num1);
		num4 = num4 + 0.00104 * Math.Sin(2 * num - num1);
		num4 = num4 - 0.00035 * Math.Sin(2 * num + num1);
		num4 = num4 - 8E-05 * ct * Math.Sin(num1);
		double num5 = num4 / Math.Sqrt(num3 - num2 * num2);
		this.mSunPositionInSkyArr[0] = num + Math.Atan(num5 / Math.Sqrt(1 - num5 * num5));
		num5 = num2 / Math.Sqrt(num3);
		this.mSunPositionInSkyArr[1] = Math.Atan(num5 / Math.Sqrt(1 - num5 * num5));
	}

	public bool CalculateSunRiseSetTimes(SunTimes.LatitudeCoords lat, SunTimes.LongitudeCoords lon, DateTime date, ref DateTime riseTime, ref DateTime setTime, ref bool isSunrise, ref bool isSunset)
	{
		bool flag = this.CalculateSunRiseSetTimes(lat.ToDouble(), lon.ToDouble(), date, ref riseTime, ref setTime, ref isSunrise, ref isSunset);
		return flag;
	}

	public bool CalculateSunRiseSetTimes(double lat, double lon, DateTime date, ref DateTime riseTime, ref DateTime setTime, ref bool isSunrise, ref bool isSunset)
	{
		bool flag;
		lock (this.mLock)
		{
			TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
			double num = (double)(-(int)Math.Round(utcOffset.TotalSeconds / 3600));
			double julianDay = this.GetJulianDay(date) - 2451545;
			if ((this.Sign(num) != this.Sign(lon) ? true : num == 0))
			{
				lon /= 360;
				double num1 = num / 24;
				double num2 = julianDay / 36525 + 1;
				double num3 = this.LocalSiderealTimeForTimeZone(lon, julianDay, num1);
				julianDay += num1;
				this.CalculateSunPosition(julianDay, num2);
				double num4 = this.mSunPositionInSkyArr[0];
				double num5 = this.mSunPositionInSkyArr[1];
				julianDay += 1;
				this.CalculateSunPosition(julianDay, num2);
				double num6 = this.mSunPositionInSkyArr[0];
				double num7 = this.mSunPositionInSkyArr[1];
				if (num6 < num4)
				{
					num6 += 6.28318530717959;
				}
				this.mIsSunrise = false;
				this.mIsSunset = false;
				this.mRightAscentionArr[0] = num4;
				this.mDecensionArr[0] = num5;
				for (int i = 0; i < 24; i++)
				{
					this.mRightAscentionArr[2] = num4 + (double)(i + 1) * (num6 - num4) / 24;
					this.mDecensionArr[2] = num5 + (double)(i + 1) * (num7 - num5) / 24;
					this.mVHzArr[2] = this.TestHour(i, num, num3, lat);
					this.mRightAscentionArr[0] = this.mRightAscentionArr[2];
					this.mDecensionArr[0] = this.mDecensionArr[2];
					this.mVHzArr[0] = this.mVHzArr[2];
				}
				riseTime = new DateTime(date.Year, date.Month, date.Day, this.mRiseTimeArr[0], this.mRiseTimeArr[1], 0);
				setTime = new DateTime(date.Year, date.Month, date.Day, this.mSetTimeArr[0], this.mSetTimeArr[1], 0);
				isSunset = true;
				isSunrise = true;
				if (!(this.mIsSunrise ? true : this.mIsSunset))
				{
					if (this.mVHzArr[2] >= 0)
					{
						isSunset = false;
					}
					else
					{
						isSunrise = false;
					}
				}
				else if (!this.mIsSunrise)
				{
					isSunrise = false;
				}
				else if (!this.mIsSunset)
				{
					isSunset = false;
				}
				flag = true;
			}
			else
			{
				Debug.Print("WARNING: time zone and longitude are incompatible!");
				flag = false;
			}
		}
		return flag;
	}

	private double GetJulianDay(DateTime date)
	{
		int month = date.Month;
		int day = date.Day;
		int year = date.Year;
		bool flag = (year < 1583 ? false : true);
		if ((month == 1 ? true : month == 2))
		{
			year--;
			month += 12;
		}
		double num = Math.Floor((double)year / 100);
		double num1 = 0;
		num1 = (!flag ? 0 : 2 - num + Math.Floor(num / 4));
		double num2 = Math.Floor(365.25 * (double)(year + 4716)) + Math.Floor(30.6001 * (double)(month + 1)) + (double)day + num1 - 1524.5;
		return num2;
	}

	private double LocalSiderealTimeForTimeZone(double lon, double jd, double z)
	{
		double num = 24110.5 + 8640184.813 * jd / 36525 + 86636.6 * z + 86400 * lon;
		num /= 86400;
		num -= Math.Floor(num);
		return num * 360 * 0.0174532925199433;
	}

	private int Sign(double value)
	{
		int num = 0;
		if (value <= 0)
		{
			num = (value >= 0 ? 0 : -1);
		}
		else
		{
			num = 1;
		}
		return num;
	}

	private double TestHour(int k, double zone, double t0, double lat)
	{
		double num;
		double[] numArray = new double[] { t0 - this.mRightAscentionArr[0] + (double)k * 0.262516168343005, default(double), t0 - this.mRightAscentionArr[2] + (double)k * 0.262516168343005 + 0.262516168343005 };
		numArray[1] = (numArray[2] + numArray[0]) / 2;
		this.mDecensionArr[1] = (this.mDecensionArr[2] + this.mDecensionArr[0]) / 2;
		double num1 = Math.Sin(lat * 0.0174532925199433);
		double num2 = Math.Cos(lat * 0.0174532925199433);
		double num3 = Math.Cos(1.58533491946401);
		if (k <= 0)
		{
			this.mVHzArr[0] = num1 * Math.Sin(this.mDecensionArr[0]) + num2 * Math.Cos(this.mDecensionArr[0]) * Math.Cos(numArray[0]) - num3;
		}
		this.mVHzArr[2] = num1 * Math.Sin(this.mDecensionArr[2]) + num2 * Math.Cos(this.mDecensionArr[2]) * Math.Cos(numArray[2]) - num3;
		if (this.Sign(this.mVHzArr[0]) != this.Sign(this.mVHzArr[2]))
		{
			this.mVHzArr[1] = num1 * Math.Sin(this.mDecensionArr[1]) + num2 * Math.Cos(this.mDecensionArr[1]) * Math.Cos(numArray[1]) - num3;
			double num4 = 2 * this.mVHzArr[0] - 4 * this.mVHzArr[1] + 2 * this.mVHzArr[2];
			double num5 = -3 * this.mVHzArr[0] + 4 * this.mVHzArr[1] - this.mVHzArr[2];
			double num6 = num5 * num5 - 4 * num4 * this.mVHzArr[0];
			if (num6 >= 0)
			{
				num6 = Math.Sqrt(num6);
				double num7 = (-num5 + num6) / (2 * num4);
				if ((num7 > 1 ? true : num7 < 0))
				{
					num7 = (-num5 - num6) / (2 * num4);
				}
				double num8 = (double)k + num7 + 0.00833333333333333;
				int num9 = (int)Math.Floor(num8);
				int num10 = (int)Math.Floor((num8 - (double)num9) * 60);
				double num11 = numArray[0] + num7 * (numArray[2] - numArray[0]);
				double num12 = -Math.Cos(this.mDecensionArr[1]) * Math.Sin(num11);
				double num13 = num2 * Math.Sin(this.mDecensionArr[1]) - num1 * Math.Cos(this.mDecensionArr[1]) * Math.Cos(num11);
				double num14 = Math.Atan2(num12, num13) / 0.0174532925199433;
				if (num14 < 0)
				{
					num14 += 360;
				}
				if ((this.mVHzArr[0] >= 0 ? false : this.mVHzArr[2] > 0))
				{
					this.mRiseTimeArr[0] = num9;
					this.mRiseTimeArr[1] = num10;
					this.mRizeAzimuth = num14;
					this.mIsSunrise = true;
				}
				if ((this.mVHzArr[0] <= 0 ? false : this.mVHzArr[2] < 0))
				{
					this.mSetTimeArr[0] = num9;
					this.mSetTimeArr[1] = num10;
					this.mSetAzimuth = num14;
					this.mIsSunset = true;
				}
				num = this.mVHzArr[2];
			}
			else
			{
				num = this.mVHzArr[2];
			}
		}
		else
		{
			num = this.mVHzArr[2];
		}
		return num;
	}

	internal abstract class Coords
	{
		protected internal int mDegrees;

		protected internal int mMinutes;

		protected internal int mSeconds;

		protected Coords()
		{
		}

		protected internal abstract int Sign();

		public double ToDouble()
		{
			double num = (double)this.Sign() * ((double)this.mDegrees + (double)this.mMinutes / 60 + (double)this.mSeconds / 3600);
			return num;
		}
	}

	public class LatitudeCoords : SunTimes.Coords
	{
		protected internal SunTimes.LatitudeCoords.Direction mDirection;

		public LatitudeCoords(int degrees, int minutes, int seconds, SunTimes.LatitudeCoords.Direction direction)
		{
			this.mDegrees = degrees;
			this.mMinutes = minutes;
			this.mSeconds = seconds;
			this.mDirection = direction;
		}

		protected internal override int Sign()
		{
			return (this.mDirection == SunTimes.LatitudeCoords.Direction.North ? 1 : -1);
		}

		public enum Direction
		{
			North,
			South
		}
	}

	public class LongitudeCoords : SunTimes.Coords
	{
		protected internal SunTimes.LongitudeCoords.Direction mDirection;

		public LongitudeCoords(int degrees, int minutes, int seconds, SunTimes.LongitudeCoords.Direction direction)
		{
			this.mDegrees = degrees;
			this.mMinutes = minutes;
			this.mSeconds = seconds;
			this.mDirection = direction;
		}

		protected internal override int Sign()
		{
			return (this.mDirection == SunTimes.LongitudeCoords.Direction.East ? 1 : -1);
		}

		public enum Direction
		{
			East,
			West
		}
	}
}