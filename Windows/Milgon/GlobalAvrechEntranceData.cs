using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Milgon
{
	public class GlobalAvrechEntranceData
	{
		public List<AvrechMonthData> AllAvrechData
		{
			get;
			set;
		}

		public GlobalAvrechEntranceData()
		{
			this.AllAvrechData = new List<AvrechMonthData>();
		}

		public void LoadData(string FilePath)
		{
			DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(GlobalAvrechEntranceData));
			FileStream fileStream = new FileStream(FilePath, FileMode.Open);
			GlobalAvrechEntranceData globalAvrechEntranceDatum = (GlobalAvrechEntranceData)dataContractSerializer.ReadObject(fileStream);
			fileStream.Close();
			this.AllAvrechData = globalAvrechEntranceDatum.AllAvrechData;
		}

		public void LoadFromDataTable(DataTable dt)
		{
			int i;
			int num = 1;
			int num1 = 3;
			DataRow item = dt.Rows[num];
			string str = "";
			for (i = 1; i < dt.Columns.Count; i++)
			{
				string str1 = item[i].ToString();
				if (str1 != "")
				{
					str = str1;
				}
				item[i] = str;
			}
			int num2 = 3;
			for (int j = 4; j < dt.Rows.Count; j++)
			{
				DataRow dataRow = dt.Rows[j];
				if (!(dataRow[1].ToString() == string.Empty))
				{
					int num3 = dataRow[1].ToInt();
					string str2 = dataRow[2].ToString();
					AvrechMonthData avrechMonthDatum = new AvrechMonthData(new int?(num3), str2);
          bool IsOnlySederA = false;
          bool IsOnlySederB = false;
          if (dataRow[0].ToString().Trim() == "א") IsOnlySederA = true;
          if (dataRow[0].ToString().Trim() == "ב") IsOnlySederB = true;

          avrechMonthDatum.IsOnlySederA = IsOnlySederA;
          avrechMonthDatum.IsOnlySederB = IsOnlySederB;

          i = num2;
					while (i < dt.Columns.Count)
					{
						dt.Rows[num1][i].ToString();
						if (!(dt.Rows[num1 - 1][i].ToString().Trim() == "סוגיות"))
						{
							bool flag = CommonLibrary.ParseBoolean(dataRow[i + 2]);
							bool flag1 = CommonLibrary.ParseBoolean(dataRow[i + 5]);
							if (!(dataRow[i].ToString().Trim() == ""))
							{
								if (dataRow[i].ToString().Trim() == "מ")
								{
									flag = true;
								}
								if (dataRow[i + 3].ToString().Trim() == "מ")
								{
									flag1 = true;
								}
								try
								{
									if (str2 == "גיספאן נתן")
									{
										if (EnteranceRecord.Parse(dataRow[i], CommonLibrary.CommonmilgaStructure.SederA, EnteranceRecord.RecordType.Enter).RecordTime.TimeOfDay <= DateTime.Parse("09:40").TimeOfDay)
										{
											dataRow[i] = "900";
										}
									}
									if (str2 == "מלמד יוסף")
									{
										if (EnteranceRecord.Parse(dataRow[i], CommonLibrary.CommonmilgaStructure.SederA, EnteranceRecord.RecordType.Enter).RecordTime.TimeOfDay <= DateTime.Parse("10:00").TimeOfDay)
										{
											dataRow[i] = "900";
										}
									}
                  avrechMonthDatum.AddDayEntry(EnteranceRecord.Parse(dataRow[i], CommonLibrary.CommonmilgaStructure.SederA, EnteranceRecord.RecordType.Enter), EnteranceRecord.Parse(dataRow[i + 1], CommonLibrary.CommonmilgaStructure.SederA, EnteranceRecord.RecordType.Exit), EnteranceRecord.Parse(dataRow[i + 3], CommonLibrary.CommonmilgaStructure.SederB, EnteranceRecord.RecordType.Enter), EnteranceRecord.Parse(dataRow[i + 4], CommonLibrary.CommonmilgaStructure.SederB, EnteranceRecord.RecordType.Exit), flag, flag1, IsOnlySederA, IsOnlySederB);
								}
								catch
								{
									throw new Exception(string.Format("{1} שגיאה בקריאת נתונים מאברך {0} עמודה מספר", str2, i));
								}
							}
							i += 6;
						}
						else
						{
							int SummaryCount = dataRow[i].ToInt();
							double kolelBoker = dataRow[i + 1].ToType<double>();
							double KolelErev = dataRow[i + 2].ToType<double>();
							int KolelShishi = dataRow[i + 3].ToInt();
              double Haborot = dataRow[i + 4].ToType<double>();
              double Sikumim = dataRow[i + 5].ToType<double>();
              double Mivhanim = dataRow[i + 6].ToType<double>();                          
              double LastMonthAdditions = dataRow[i + 7].ToType<double>();
							double Additions = dataRow[i + 8].ToType<double>();
							double Others = dataRow[i + 9].ToType<double>();
							double MinutesReduction = 0;
							bool IsIgnoreHours = CommonLibrary.ParseBoolean(dataRow[i + 8]);
							avrechMonthDatum.AddGlobalAdditions(SummaryCount, kolelBoker, KolelShishi, LastMonthAdditions, Additions, Others, MinutesReduction, KolelErev, IsIgnoreHours, Haborot,Sikumim, Mivhanim);
							break;
						}
					}
					this.AllAvrechData.Add(avrechMonthDatum);
				}
			}
		}

		public void SaveData(string FilePath)
		{
			FileStream fileStream = new FileStream(FilePath, FileMode.Create);
			(new DataContractSerializer(typeof(GlobalAvrechEntranceData))).WriteObject(fileStream, this);
			fileStream.Close();
		}
	}
}