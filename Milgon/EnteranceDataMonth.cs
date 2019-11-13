using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Milgon
{
	[DataContract]
	public class EnteranceDataMonth
	{
		private TimeSpan _TotalTime;

		[DataMember]
		public int CountGotBonusSederA
		{
			get;
			private set;
		}

		[DataMember]
		public int CountGotBonusSederB
		{
			get;
			private set;
		}

		[DataMember]
		public int CountLateSederA
		{
			get;
			private set;
		}

    public int CountLateSederAOnly
    {
      get
      {
        return this.CountLateSederA - this.CountSederA_Absence;
      }    
    }

    [DataMember]
		public int CountLateSederA_Above30Min
		{
			get;
			private set;
		}

		[DataMember]
		public string CountLateSederA_Above30Min_TimeRecords_String
		{
			get;
			private set;
		}

		[DataMember]
		public int CountLateSederB
		{
			get;
			private set;
		}

    public int CountLateSederBOnly
    {
      get
      {
        return this.CountLateSederB - this.CountSederB_Absence;
      }
    }

    [DataMember]
		public int CountLateSederB_Above30Min
		{
			get;
			private set;
		}

		[DataMember]
		public string CountLateSederB_Above30Min_TimeRecords_String
		{
			get;
			private set;
		}

		[DataMember]
		public int CountNotGotBonusSederA
		{
			get;
			private set;
		}

		[DataMember]
		public int CountNotGotBonusSederB
		{
			get;
			private set;
		}

		[DataMember]
		public int CountNotTookAllowedBreakSederA
		{
			get;
			private set;
		}

    public int CountNotTookAllowedBreakSederAOnly
    {
      get
      {
        return CountNotTookAllowedBreakSederA - CountLateSederAOnly - CountSederA_Absence;
      }    
    }

    [DataMember]
		public int CountNotTookAllowedBreakSederB
		{
			get;
			private set;
		}

    public int CountNotTookAllowedBreakSederBOnly
    {
      get
      {
        return CountNotTookAllowedBreakSederB - CountLateSederBOnly - CountSederB_Absence; 
      }
    }

    [DataMember]
		public int CountSederA_Absence
		{
			get;
			private set;
		}

		[DataMember]
		public int CountSederA_Absence_NotApproved
		{
			get;
			private set;
		}

		[DataMember]
		public int CountSederA_Presence
		{
			get;
			private set;
		}

		[DataMember]
		public int CountSederB_Absence
		{
			get;
			private set;
		}

		[DataMember]
		public int CountSederB_Absence_NotApproved
		{
			get;
			private set;
		}

		[DataMember]
		public int CountSederB_Presence
		{
			get;
			private set;
		}

		[DataMember]
		public int CountViolateBonusSederA
		{
			get;
			private set;
		}

		[DataMember]
		public int CountViolateBonusSederB
		{
			get;
			private set;
		}

		[DataMember]
		public List<EnteranceDataDay> Days
		{
			get;
			set;
		}

    public int CountIsApprovedInOrOutSederA
    {
      get;
      set;
    }

    public int CountIsApprovedInOrOutSederB
    {
      get;
      set;
    }

    [DataMember]
		public TimeSpan TotalApprovedTime
		{
			get;
			private set;
		}

		[DataMember]
		public TimeSpan TotalMissingTime
		{
			get;
			private set;
		}

		[DataMember]
		public TimeSpan TotalTime
		{
			get
			{
				return this._TotalTime;
			}
			private set
			{
				this._TotalTime = value;
			}
		}

		public EnteranceDataMonth()
		{
			this.Days = new List<EnteranceDataDay>();
		}

		public void AddEntry(EnteranceRecord InA, EnteranceRecord OutA, EnteranceRecord InB, EnteranceRecord OutB, bool IsTookAllowedBreakA = false, bool IsTookAllowedBreakB = false)
		{
			EnteranceDataDay enteranceDataDay = new EnteranceDataDay();
			DateTime recordTime = InA.RecordTime;
			enteranceDataDay.DayDate = DateTime.Parse(recordTime.ToShortDateString());
			enteranceDataDay.AddEntry(SederType.A, InA, OutA, IsTookAllowedBreakA);
			enteranceDataDay.AddEntry(SederType.B, InB, OutB, IsTookAllowedBreakB);
			this.Days.Add(enteranceDataDay);
			this.UpdateTotalTime();
		}

    private void UpdateTotalTime()
    {
      this._TotalTime = new TimeSpan();
      TimeSpan timeSpan1 = new TimeSpan();
      this.TotalApprovedTime = timeSpan1;
      timeSpan1 = new TimeSpan();
      this.TotalMissingTime = timeSpan1;
      foreach (EnteranceDataDay enteranceDataDay in this.Days)
      {
        this._TotalTime += enteranceDataDay.TotalTime;
        EnteranceDataMonth totalApprovedTime = this;
        totalApprovedTime.TotalApprovedTime = totalApprovedTime.TotalApprovedTime + enteranceDataDay.ApprovedTime;
        EnteranceDataMonth totalMissingTime = this;
        totalMissingTime.TotalMissingTime = totalMissingTime.TotalMissingTime + enteranceDataDay.MissingTime;
      }
      this.CountSederA_Presence = (
        from day in this.Days
        where day.SederA.Out.RecordTime > day.SederA.In.RecordTime
        select day).Count<EnteranceDataDay>();
      this.CountSederA_Absence = this.Days.Count - this.CountSederA_Presence;
      int num = (
        from day in this.Days
        where (day.SederA.Out.RecordTime > day.SederA.In.RecordTime ? true : day.SederA.In.IsApproved)
        select day).Count<EnteranceDataDay>();
      this.CountSederA_Absence_NotApproved = this.Days.Count - num;
      this.CountSederB_Presence = (
        from day in this.Days
        where day.SederB.Out.RecordTime > day.SederB.In.RecordTime
        select day).Count<EnteranceDataDay>();
      this.CountSederB_Absence = this.Days.Count - this.CountSederB_Presence;
      int num1 = (
        from day in this.Days
        where (day.SederB.Out.RecordTime > day.SederB.In.RecordTime ? true : day.SederB.In.IsApproved)
        select day).Count<EnteranceDataDay>();
      this.CountSederB_Absence_NotApproved = this.Days.Count - num1;
      this.CountLateSederA = (
        from day in this.Days
        where day.SederA.IsLate
        select day).Count<EnteranceDataDay>();
      this.CountLateSederA_Above30Min = this.Days.Where<EnteranceDataDay>((EnteranceDataDay day) =>
      {
        bool timeOfDay;
        if (day.SederA.In.RecordTime.TimeOfDay <= CommonLibrary.CommonmilgaStructure.SederA.StartTime.Value.AddMinutes(30).TimeOfDay)
        {
          timeOfDay = false;
        }
        else
        {
          DateTime recordTime = day.SederA.In.RecordTime;
          TimeSpan timeSpan = recordTime.TimeOfDay;
          recordTime = CommonLibrary.CommonmilgaStructure.SederA.EndTime.Value;
          timeOfDay = timeSpan != recordTime.TimeOfDay;
        }
        return timeOfDay;
      }).Count<EnteranceDataDay>();
      string[] array = this.Days.Where<EnteranceDataDay>((EnteranceDataDay day) =>
      {
        bool timeOfDay;
        if (day.SederA.In.RecordTime.TimeOfDay <= CommonLibrary.CommonmilgaStructure.SederA.StartTime.Value.AddMinutes(30).TimeOfDay)
        {
          timeOfDay = false;
        }
        else
        {
          DateTime recordTime = day.SederA.In.RecordTime;
          TimeSpan timeSpan = recordTime.TimeOfDay;
          recordTime = CommonLibrary.CommonmilgaStructure.SederA.EndTime.Value;
          timeOfDay = timeSpan != recordTime.TimeOfDay;
        }
        return timeOfDay;
      }).Select<EnteranceDataDay, string>((EnteranceDataDay day) => day.SederA.In.RecordTime.ToString("HH:mm")).ToArray<string>();
      this.CountLateSederA_Above30Min_TimeRecords_String = string.Join(",\r\n", array);
      this.CountLateSederB = (
        from day in this.Days
        where day.SederB.IsLate
        select day).Count<EnteranceDataDay>();
      this.CountLateSederB_Above30Min = this.Days.Where<EnteranceDataDay>((EnteranceDataDay day) =>
      {
        bool timeOfDay;
        if (day.SederB.In.RecordTime.TimeOfDay <= CommonLibrary.CommonmilgaStructure.SederB.StartTime.Value.AddMinutes(30).TimeOfDay)
        {
          timeOfDay = false;
        }
        else
        {
          DateTime recordTime = day.SederB.In.RecordTime;
          TimeSpan timeSpan = recordTime.TimeOfDay;
          recordTime = CommonLibrary.CommonmilgaStructure.SederB.EndTime.Value;
          timeOfDay = timeSpan != recordTime.TimeOfDay;
        }
        return timeOfDay;
      }).Count<EnteranceDataDay>();
      string[] strArrays = this.Days.Where<EnteranceDataDay>((EnteranceDataDay day) =>
      {
        bool timeOfDay;
        if (day.SederB.In.RecordTime.TimeOfDay <= CommonLibrary.CommonmilgaStructure.SederB.StartTime.Value.AddMinutes(30).TimeOfDay)
        {
          timeOfDay = false;
        }
        else
        {
          DateTime recordTime = day.SederB.In.RecordTime;
          TimeSpan timeSpan = recordTime.TimeOfDay;
          recordTime = CommonLibrary.CommonmilgaStructure.SederB.EndTime.Value;
          timeOfDay = timeSpan != recordTime.TimeOfDay;
        }
        return timeOfDay;
      }).Select<EnteranceDataDay, string>((EnteranceDataDay day) => day.SederB.In.RecordTime.ToString("HH:mm")).ToArray<string>();
      this.CountLateSederB_Above30Min_TimeRecords_String = string.Join(",\r\n", strArrays);
      this.CountGotBonusSederA = (
        from day in this.Days
        where day.SederA.IsGotBonus
        select day).Count<EnteranceDataDay>();
      this.CountGotBonusSederB = (
        from day in this.Days
        where day.SederB.IsGotBonus
        select day).Count<EnteranceDataDay>();
      this.CountViolateBonusSederA = (
        from day in this.Days
        where day.SederA.IsViolateBonus
        select day).Count<EnteranceDataDay>();
      this.CountViolateBonusSederB = (
        from day in this.Days
        where day.SederB.IsViolateBonus
        select day).Count<EnteranceDataDay>();
      this.CountNotGotBonusSederA = (
        from day in this.Days
        where !day.SederA.IsGotBonus
        select day).Count<EnteranceDataDay>();
      this.CountNotGotBonusSederB = (
        from day in this.Days
        where !day.SederB.IsGotBonus
        select day).Count<EnteranceDataDay>();
      this.CountNotTookAllowedBreakSederA = (
        from day in this.Days
        where !day.SederA.IsTookAllowedBreak
        select day).Count<EnteranceDataDay>();
      this.CountNotTookAllowedBreakSederB = (
        from day in this.Days
        where !day.SederB.IsTookAllowedBreak
        select day).Count<EnteranceDataDay>();

      CountIsApprovedInOrOutSederA = this.Days.Where(x => x.SederA.IsApprovedInOrOut).Count();
      CountIsApprovedInOrOutSederB = this.Days.Where(x => x.SederB.IsApprovedInOrOut).Count();
    }
	}
}