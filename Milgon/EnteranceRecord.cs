using System;
using System.Runtime.Serialization;

namespace Milgon
{
	[DataContract]
	public class EnteranceRecord
	{
		private DateTime _RecordTime = new DateTime();

		private bool _IsApproved = false;

		[DataMember]
		public bool IsApproved
		{
			get
			{
				return this._IsApproved;
			}
			set
			{
				this._IsApproved = value;
			}
		}

		[DataMember]
		public DateTime RecordTime
		{
			get
			{
				return this._RecordTime;
			}
			set
			{
				this._RecordTime = value;
			}
		}

		public EnteranceRecord()
		{
		}

		public EnteranceRecord(DateTime RecordTime, bool IsApproved = false)
		{
			this.RecordTime = RecordTime;
			this.IsApproved = IsApproved;
		}

		public static EnteranceRecord Parse(object o, Seder seder, EnteranceRecord.RecordType recordType)
		{
			DateTime? nullable;
			DateTime value;
			string str = o.ToString();
			EnteranceRecord enteranceRecord = new EnteranceRecord();
			if ((str.Contains("M") ? true : str.Contains("מ")))
			{
				enteranceRecord.IsApproved = true;
			}
			if (!(str.Trim() == "מ"))
			{
				if (recordType != EnteranceRecord.RecordType.Enter)
				{
					value = seder.EndTime.Value;
					nullable = new DateTime?(DateTime.Parse(value.ToShortTimeString()));
				}
				else
				{
					value = seder.StartTime.Value;
					nullable = new DateTime?(DateTime.Parse(value.ToShortTimeString()));
				}
				enteranceRecord.RecordTime = CommonLibrary.ParseTime(str, nullable);
			}
			else
			{
				value = seder.EndTime.Value;
				enteranceRecord.RecordTime = DateTime.Parse(value.ToShortTimeString());
			}
			return enteranceRecord;
		}

		public enum RecordType
		{
			Enter,
			Exit
		}
	}
}