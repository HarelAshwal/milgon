using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Milgon
{
	[DataContract]
	public class AvrechMonthData
	{
		[DataMember]
		public TimeSpan _MissingTime;

		private int _KolelShishiCount;

		private double _KolelShishiTotal;

		private double _KolelErev;

		private double _LastMonthAdditions;

		private double _Additions;

		private double _Others;

		private double _MinutesReduction;

		private bool _IsIgnoreHours;

		private TimeSpan _ExpectedHours;

		[DataMember]
		public double Additions
		{
			get
			{
				return this._Additions;
			}
			set
			{
				this._Additions = value;
				this.CalculateMilga();
			}
		}

		[DataMember]
		public TimeSpan ApprovedTime
		{
			get;
			private set;
		}

		[DataMember]
		public double BonusSederA
		{
			get;
			private set;
		}
   

    [DataMember]
		public double BonusSederB
		{
			get;
			private set;
		}

		public double BonusSederTotal
		{
			get;
			private set;
		}

		[DataMember]
		public double BonusSummary
		{
			get;
			set;
		}

		[DataMember]
		public EnteranceDataMonth entranceDataMonth
		{
			get;
			private set;
		}

		public TimeSpan ExpectedHours
		{
			get
			{
				return this._ExpectedHours;
			}
			private set
			{
				this._ExpectedHours = value;
			}
		}

		[DataMember]
		public int? Id
		{
			get;
			private set;
		}

		[DataMember]
		public bool IsIgnoreHours
		{
			get
			{
				return this._IsIgnoreHours;
			}
			set
			{
				this._IsIgnoreHours = value;
				this.CalculateMilga();
			}
		}

		[DataMember]
		public double KolelBokerAdditions
		{
			get;
			set;
		}

		[DataMember]
		public double KolelErev
		{
			get
			{
				return this._KolelErev;
			}
			set
			{
				this._KolelErev = value;
				this.CalculateMilga();
			}
		}

		[DataMember]
		public int KolelShishiCount
		{
			get
			{
				return this._KolelShishiCount;
			}
			set
			{
				this._KolelShishiCount = value;
				this.CalculateMilga();
			}
		}

		[DataMember]
		public double KolelShishiTotal
		{
			get
			{
				return this._KolelShishiTotal;
			}
			set
			{
				this._KolelShishiTotal = value;
			}
		}

		[DataMember]
		public double LastMonthAdditions
		{
			get
			{
				return this._LastMonthAdditions;
			}
			set
			{
				this._LastMonthAdditions = value;
				this.CalculateMilga();
			}
		}

    double _Haborot;
    [DataMember]
    public double Haborot { get => _Haborot; set => _Haborot = value; }

    double _Sikumim;

    [DataMember]
    public double Sikumim { get => _Sikumim; set => _Sikumim = value; }

    double _Mivhanim;

    [DataMember]
    public double Mivhanim { get => _Mivhanim; set => _Mivhanim = value; }

    [DataMember]
		public double MilgaAmount
		{
			get;
			set;
		}

		[DataMember]
		public double MilgaAmountBase
		{
			get;
			set;
		}

		[DataMember]
		public double MilgaAmountBaseAndBonus
		{
			get;
			set;
		}

		[DataMember]
		public double MinutesReduction
		{
			get
			{
				return this._MinutesReduction;
			}
			set
			{
				this._MinutesReduction = value;
				this.CalculateMilga();
			}
		}

		[DataMember]
		public double MissingHoursReduction
		{
			get;
			private set;
		}

		public TimeSpan MissingTime
		{
			get
			{
				return this._MissingTime;
			}
			set
			{
				this._MissingTime = value;
				TimeSpan missingTime = this.MissingTime;
				double totalMilliseconds = missingTime.TotalMilliseconds * 100;
				missingTime = this.ExpectedHours;
				this.NonPresencePrecentage = totalMilliseconds / missingTime.TotalMilliseconds;
				this.PresencePrecentage = 100 - this.NonPresencePrecentage;
			}
		}

		[DataMember]
		public string Name
		{
			get;
			set;
		}

		[DataMember]
		public double NonPresencePrecentage
		{
			get;
			private set;
		}

		[DataMember]
		public double Others
		{
			get
			{
				return this._Others;
			}
			set
			{
				this._Others = value;
				this.CalculateMilga();
			}
		}

		[DataMember]
		public double PresencePrecentage
		{
			get;
			private set;
		}

		[DataMember]
		public int SummaryCount
		{
			get;
			private set;
		}

		[DataMember]
		public TimeSpan TotalTime
		{
			get;
			private set;
		}

    public AvrechMonthData() : this(null, "")
		{
		}

		public AvrechMonthData(int? Id, string Name = "")
		{
			this.Id = Id;
			this.Name = Name;
			this.entranceDataMonth = new EnteranceDataMonth();
			this.MissingHoursReduction = 0;
			this.BonusSederA = 0;
			this.BonusSederB = 0;
		}

		public void AddDayEntry(EnteranceRecord InA, EnteranceRecord OutA, EnteranceRecord InB, EnteranceRecord OutB, bool IsTookAllowedBreakA = false, bool IsTookAllowedBreakB = false)
		{
			this.entranceDataMonth.AddEntry(InA, OutA, InB, OutB, IsTookAllowedBreakA, IsTookAllowedBreakB);
			this.CalculateMilga();
		}

		public void AddGlobalAdditions(int SummaryCount, double KolelBokerAdditions, int KolelShishiAdditions, double LastMonthAdditions, double Additions,
      double Others, double MinutesReduction, double KolelErev, bool IsIgnoreHours , double Haborot, double Sikumim, double Mivhanim)
		{
			this.SummaryCount = SummaryCount;
			this.KolelBokerAdditions = KolelBokerAdditions;
			this.KolelShishiCount = KolelShishiAdditions;
			this.LastMonthAdditions = LastMonthAdditions;
			this.KolelErev = KolelErev;
			this.Additions = Additions;
			this.Others = Others;
			this.MinutesReduction = MinutesReduction;
			this.IsIgnoreHours = IsIgnoreHours;
      this.Haborot = Haborot;
      this.Sikumim = Sikumim;
      this.Mivhanim = Mivhanim;
			this.CalculateMilga();
		}

		private void CalculateMilga()
		{
			if (this.entranceDataMonth != null)
			{
				this.TotalTime = this.entranceDataMonth.TotalTime;
				this.ApprovedTime = this.entranceDataMonth.TotalApprovedTime;
				if (this.MinutesReduction > 0)
				{
					AvrechMonthData totalTime = this;
					totalTime.TotalTime = totalTime.TotalTime - TimeSpan.FromMinutes((double)this.entranceDataMonth.CountSederA_Presence * this.MinutesReduction);
				}
				MilgaStructure commonmilgaStructure = CommonLibrary.CommonmilgaStructure;
				double? basicMilga = commonmilgaStructure.BasicMilga;
				this.MilgaAmount = (double)basicMilga.Value;
				if ((this.TotalTime != TimeSpan.Zero ? false : this.ApprovedTime == TimeSpan.Zero))
				{
					this.MilgaAmount = 0;
				}
				TimeSpan totalDayTime = commonmilgaStructure.GetTotalDayTime();
				this.ExpectedHours = TimeSpan.FromHours(totalDayTime.TotalHours * (double)this.entranceDataMonth.Days.Count);
				this.MissingTime = this.entranceDataMonth.TotalMissingTime;
				double totalHours = this.MissingTime.TotalHours;
				basicMilga = commonmilgaStructure.MissingHourFine;
				this.MissingHoursReduction = totalHours * (double)basicMilga.Value;
				AvrechMonthData milgaAmount = this;
				milgaAmount.MilgaAmount = milgaAmount.MilgaAmount - this.MissingHoursReduction;
				this.MilgaAmountBase = this.MilgaAmount;
				this.BonusSederTotal = 0;
				int countViolateBonusSederA = this.entranceDataMonth.CountViolateBonusSederA;
				int lateCountForBonusCanceling = commonmilgaStructure.SederA.LateCountForBonusCanceling.Value;
				if (countViolateBonusSederA >= lateCountForBonusCanceling)
				{
					this.BonusSederA = 0;
				}
				else
				{
					basicMilga = commonmilgaStructure.SederA.Bonus;
					this.BonusSederA = (double)basicMilga.Value * (double)this.entranceDataMonth.CountGotBonusSederA;
					AvrechMonthData bonusSederTotal = this;
					bonusSederTotal.BonusSederTotal = bonusSederTotal.BonusSederTotal + this.BonusSederA;
				}
				countViolateBonusSederA = this.entranceDataMonth.CountViolateBonusSederB;
				lateCountForBonusCanceling = commonmilgaStructure.SederB.LateCountForBonusCanceling.Value;
				if (countViolateBonusSederA >= lateCountForBonusCanceling)
				{
					this.BonusSederB = 0;
				}
				else
				{
					basicMilga = commonmilgaStructure.SederB.Bonus;
					this.BonusSederB = (double)basicMilga.Value * (double)this.entranceDataMonth.CountGotBonusSederB;
					AvrechMonthData avrechMonthDatum = this;
					avrechMonthDatum.BonusSederTotal = avrechMonthDatum.BonusSederTotal + this.BonusSederB;
				}
				AvrechMonthData milgaAmount1 = this;
				milgaAmount1.MilgaAmount = milgaAmount1.MilgaAmount + this.BonusSederTotal;
				this.MilgaAmountBaseAndBonus = this.MilgaAmount;
				basicMilga = commonmilgaStructure.SummaryBonus;
				this.BonusSummary = (double)basicMilga.Value * (double)this.SummaryCount;
				AvrechMonthData avrechMonthDatum1 = this;
				avrechMonthDatum1.MilgaAmount = avrechMonthDatum1.MilgaAmount + this.BonusSummary;
				if (this.IsIgnoreHours)
				{
					this.MilgaAmount = 0;
				}
				AvrechMonthData milgaAmount2 = this;
				milgaAmount2.MilgaAmount = milgaAmount2.MilgaAmount + this.KolelBokerAdditions;
				basicMilga = commonmilgaStructure.KolelShishiMilga;
				this.KolelShishiTotal = (double)basicMilga.Value * (double)this.KolelShishiCount;
				AvrechMonthData avrechMonthDatum2 = this;
				avrechMonthDatum2.MilgaAmount = avrechMonthDatum2.MilgaAmount + this.KolelShishiTotal;
				AvrechMonthData milgaAmount3 = this;
				milgaAmount3.MilgaAmount = milgaAmount3.MilgaAmount + this.KolelErev;
				AvrechMonthData avrechMonthDatum3 = this;
				avrechMonthDatum3.MilgaAmount = avrechMonthDatum3.MilgaAmount + this.LastMonthAdditions;
				AvrechMonthData milgaAmount4 = this;
				milgaAmount4.MilgaAmount = milgaAmount4.MilgaAmount + this.Additions;
				AvrechMonthData avrechMonthDatum4 = this;
				avrechMonthDatum4.MilgaAmount = avrechMonthDatum4.MilgaAmount + this.Others;

        // if Avrech is in 80% of time, add the bonous
        if (avrechMonthDatum4.PresencePrecentage >= 80)
        {
          avrechMonthDatum4.MilgaAmount += (this.Haborot + this.Sikumim + this.Mivhanim);
        }

        this.MilgaAmount = (double)CommonLibrary.GetUpper10(this.MilgaAmount);
			}
		}

		public override string ToString()
		{
			string str = string.Format("{0} : {1}", this.Id, this.Name);
			return str;
		}
	}
}