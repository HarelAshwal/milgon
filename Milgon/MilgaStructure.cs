using System;
using System.Runtime.CompilerServices;

namespace Milgon
{
	public class MilgaStructure
	{
		public double? BasicMilga 
		{
			get;
			set;
		}

		public double? KolelShishiMilga
		{
			get;
			set;
		}

		public double? MissingHourFine
		{
			get;
			set;
		}

		public Seder SederA
		{
			get;
			set;
		}

		public Seder SederB
		{
			get;
			set;
		}

		public double? SummaryBonus
		{
			get;
			set;
		}

		public MilgaStructure()
		{
		}

		public TimeSpan GetTotalDayTime()
		{
			TimeSpan totalSederTime = this.SederA.GetTotalSederTime() + this.SederB.GetTotalSederTime();
			return totalSederTime;
		}
	}
}