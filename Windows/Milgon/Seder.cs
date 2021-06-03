using System;
using System.Runtime.CompilerServices;

namespace Milgon
{
	public class Seder
	{
		public double? BasicMilga
		{
			get;
			set;
		}

		public double? Bonus
		{
			get;
			set;
		}

		public DateTime? EndTime
		{
			get;
			set;
		}

		public int? LateCountForBonusCanceling
		{
			get;
			set;
		}

		public DateTime? StartTime
		{
			get;
			set;
		}

		public DateTime? StartTimeBonus
		{
			get;
			set;
		}

		public Seder()
		{
		}

		public TimeSpan GetTotalSederTime()
		{
			return this.EndTime.Value - this.StartTime.Value;
		}
	}
}