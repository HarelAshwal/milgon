using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Milgon
{
	[DataContract]
	public class EnteranceEntry
	{
		private EnteranceRecord _In = new EnteranceRecord();

		private EnteranceRecord _Out = new EnteranceRecord();

		private bool _IsTookAllowedBreak = false;

		[DataMember]
		public TimeSpan ApprovedTime
		{
			get;
			set;
		}

		[DataMember]
		public EnteranceRecord In
		{
			get
			{
				return this._In;
			}
			set
			{
				this._In = value;
				this.UpdateStatistics();
			}
		}

		[DataMember]
		public bool IsAbsence
		{
			get;
			set;
		}

		[DataMember]
		public bool IsGotBonus
		{
			get;
			set;
		}

		[DataMember]
		public bool IsLate
		{
			get;
			set;
		}

    public bool IsApprovedInOrOut
    {
      get
      {
        return In.IsApproved | Out.IsApproved;
      }
    }

		[DataMember]
		public bool IsTookAllowedBreak
		{
			get
			{
				return this._IsTookAllowedBreak;
			}
			set
			{
				this._IsTookAllowedBreak = value;
				this.UpdateStatistics();
			}
		}

		[DataMember]
		public bool IsViolateBonus
		{
			get;
			set;
		}

		[DataMember]
		public TimeSpan MissingTime
		{
			get;
			set;
		}

		[DataMember]
		public EnteranceRecord Out
		{
			get
			{
				return this._Out;
			}
			set
			{
				this._Out = value;
				this.UpdateStatistics();
			}
		}

		[DataMember]
		public SederType seder
		{
			get;
			set;
		}

		[DataMember]
		public TimeSpan TotalTime
		{
			get;
			set;
		}

		public EnteranceEntry()
		{
		}

		public override string ToString()
		{
			DateTime recordTime = this.In.RecordTime;
			string shortTimeString = recordTime.ToShortTimeString();
			recordTime = this.Out.RecordTime;
			string str = string.Format("In : {0}, Out {1}, Late {2}", shortTimeString, recordTime.ToShortTimeString(), this.IsLate);
			return str;
		}

		private void UpdateStatistics()
		{
			TimeSpan timeSpan;
			TimeSpan totalSederTime;
			DateTime recordTime;
			bool flag;
			if ((this.Out == null ? false : this.In != null))
			{
				this.TotalTime = this.Out.RecordTime - this.In.RecordTime;
				MilgaStructure commonmilgaStructure = CommonLibrary.CommonmilgaStructure;
				Seder sederB = null;
				if (this.seder != SederType.A)
				{
					sederB = commonmilgaStructure.SederB;
					totalSederTime = CommonLibrary.CommonmilgaStructure.SederB.GetTotalSederTime();
					timeSpan = TimeSpan.FromHours(totalSederTime.TotalHours);
				}
				else
				{
					sederB = commonmilgaStructure.SederA;
					totalSederTime = CommonLibrary.CommonmilgaStructure.SederA.GetTotalSederTime();
					timeSpan = TimeSpan.FromHours(totalSederTime.TotalHours);
				}
				TimeSpan timeOfDay = new TimeSpan();
				TimeSpan timeOfDay1 = new TimeSpan();
				if (this.In.IsApproved)
				{
					recordTime = this.In.RecordTime;
					TimeSpan timeSpan1 = recordTime.TimeOfDay;
					recordTime = sederB.StartTime.Value;
					timeOfDay = timeSpan1 - recordTime.TimeOfDay;
				}
				if (this.Out.IsApproved)
				{
					recordTime = sederB.EndTime.Value;
					TimeSpan timeOfDay2 = recordTime.TimeOfDay;
					recordTime = this.Out.RecordTime;
					timeOfDay1 = timeOfDay2 - recordTime.TimeOfDay;
				}
				this.ApprovedTime = timeOfDay + timeOfDay1;
				this.MissingTime = timeSpan - (this.TotalTime + this.ApprovedTime);
				recordTime = this.In.RecordTime;
				TimeSpan timeSpan2 = recordTime.TimeOfDay;
				recordTime = sederB.EndTime.Value;
				this.IsAbsence = timeSpan2 == recordTime.TimeOfDay;
				this.IsLate = (this.In.RecordTime.TimeOfDay <= sederB.StartTimeBonus.Value.TimeOfDay ? false : !this.In.IsApproved);
				if (this.IsLate || !this.IsTookAllowedBreak)
				{
					flag = false;
				}
				else
				{
					flag = (this.In.IsApproved ? false : !this.Out.IsApproved);
				}
				this.IsGotBonus = flag;
				this.IsViolateBonus = (this.IsLate ? true : !this.IsTookAllowedBreak);
			}
		}
	}
}