using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Milgon
{
	internal static class CommonLibrary
	{
		private static DataContractSerializer dcs;

		public static MilgaStructure CommonmilgaStructure
		{
			get;
			set;
		}

		static CommonLibrary()
		{
			CommonLibrary.dcs = new DataContractSerializer(typeof(MilgaStructure));
			CommonLibrary.CommonmilgaStructure = CommonLibrary.GetmilgaStructure();
		}

		private static MilgaStructure GetmilgaStructure()
		{
			string directoryName = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName;
			FileStream fileStream = new FileStream(string.Concat(directoryName, "\\milgaStructure.xml"), FileMode.Open);
			MilgaStructure milgaStructure = (MilgaStructure)CommonLibrary.dcs.ReadObject(fileStream);
			fileStream.Close();
			return milgaStructure;
		}

		public static int GetUpper10(double x)
		{
			int num = (int)(Math.Ceiling((decimal)((double)x) / new decimal(10)) * new decimal(10));
			return num;
		}

		public static DataTable LoadExcelFile(string FilePath)
		{
			int i;
			DataTable dataTable = new DataTable();
			ExcelPackage excelPackage = new ExcelPackage(new FileInfo(FilePath));
			try
			{
				ExcelWorksheet item = excelPackage.Workbook.Worksheets["Data"];
				for (i = item.Dimension.Start.Column; i < item.Dimension.End.Column; i++)
				{
					dataTable.Columns.Add();
				}
				int num = 0;
				int num1 = 0;
				for (int j = item.Dimension.Start.Row; j < item.Dimension.End.Row; j++)
				{
					dataTable.Rows.Add(new object[0]);
					for (i = item.Dimension.Start.Column; i < item.Dimension.End.Column; i++)
					{
						dataTable.Rows[num][num1] = item.Cells[j, i].Value;
						num1++;
					}
					num1 = 0;
					num++;
				}
			}
			finally
			{
				if (excelPackage != null)
				{
					((IDisposable)excelPackage).Dispose();
				}
			}
			return dataTable;
		}

		public static bool ParseBoolean(object obj)
		{
			bool flag;
			flag = (!(obj.ToString().Trim() == "1") ? false : true);
			return flag;
		}

		public static DateTime ParseTime(object Time, DateTime? DefaultTime)
		{
			DateTime dateTime;
			string str = Time.ToString().Trim();
			if ((str == "מ" ? false : !(str == "M")))
			{
				string str1 = str.Replace("M", "").Replace("מ", "");
				str1 = str1.PadLeft(4, '0');
				dateTime = DateTime.Parse(str1.Insert(2, ":"));
			}
			else
			{
				dateTime = DateTime.Parse(DefaultTime.Value.ToShortTimeString());
			}
			return dateTime;
		}

		internal static void SavemilgaStructure(MilgaStructure milgaStructure)
		{
			FileStream fileStream = new FileStream("milgaStructure.xml", FileMode.Create);
			CommonLibrary.dcs.WriteObject(fileStream, milgaStructure);
			fileStream.Close();
			CommonLibrary.CommonmilgaStructure = milgaStructure;
		}
	}
}