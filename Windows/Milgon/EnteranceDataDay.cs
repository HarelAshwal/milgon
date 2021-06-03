using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Milgon
{
	[DataContract]
	public class EnteranceDataDay
	{
		private TimeSpan _TotalTime;

    [DataMember]
    public bool IsOnlySederA { get; set; }

    [DataMember]
    public bool IsOnlySederB { get; set; }


    private TimeSpan _ApprovedTime;

		private TimeSpan _MissingTime;

		[DataMember]
		public TimeSpan ApprovedTime
		{
			get
			{
        if (IsOnlySederA) return SederA.ApprovedTime;
        if (IsOnlySederB) return SederB.ApprovedTime;

        return this.SederA.ApprovedTime + this.SederB.ApprovedTime;
			}
			private set
			{
				this._ApprovedTime = value;
			}
		}

		[DataMember]
		public DateTime DayDate
		{
			get;
			set;
		}

		[DataMember]
		public TimeSpan MissingTime
		{
			get
			{
        if (IsOnlySederA) return SederA.MissingTime;
        if (IsOnlySederB) return SederB.MissingTime;
        return this.SederA.MissingTime + this.SederB.MissingTime;
			}
			private set
			{
				this._MissingTime = value;
			}
		}

		[DataMember]
		public EnteranceEntry SederA
		{
			get;
			set;
		}

		[DataMember]
		public EnteranceEntry SederB
		{
			get;
			set;
		}

		[DataMember]
		public TimeSpan TotalTime
		{
			get
			{
				this._TotalTime = this.SederA.TotalTime + this.SederB.TotalTime;
        if (IsOnlySederA) _TotalTime = SederA.TotalTime;
        if (IsOnlySederB) _TotalTime = SederB.TotalTime;
        return this._TotalTime;
			}
			private set
			{
				this._TotalTime = value;
			}
		}

		public EnteranceDataDay()
		{
		}

		public void AddEntry(SederType seder, EnteranceRecord In, EnteranceRecord Out, bool IsTookAllowedBreak = false)
		{
			switch (seder)
			{
				case SederType.A:
				{
					EnteranceEntry enteranceEntry = new EnteranceEntry()
					{
						seder = seder,
						In = In,
						Out = Out,
						IsTookAllowedBreak = IsTookAllowedBreak
					};
					this.SederA = enteranceEntry;
					break;
				}
				case SederType.B:
				{
					EnteranceEntry enteranceEntry1 = new EnteranceEntry()
					{
						seder = seder,
						In = In,
						Out = Out,
						IsTookAllowedBreak = IsTookAllowedBreak
					};
					this.SederB = enteranceEntry1;
					break;
				}
			}
		}

		public override string ToString()
		{
			DateTime dayDate = this.DayDate;
			string str = string.Format("{0} : A - {1} , B - {2}", dayDate.ToShortDateString(), this.SederA.TotalTime, this.SederB.TotalTime);
			return str;
		}
	}
}