using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace Milgon
{
	public static class CommonLibraryExtensions
	{
		public static DateTime? ClearSeconds(this DateTime? time)
		{
			DateTime value = time.Value;
			int year = value.Year;
			value = time.Value;
			int month = value.Month;
			value = time.Value;
			int day = value.Day;
			value = time.Value;
			int hour = value.Hour;
			value = time.Value;
			DateTime? nullable = new DateTime?(new DateTime(year, month, day, hour, value.Minute, 0));
			return nullable;
		}

		public static void ImportColumn(this DataTable dtTarget, DataTable dtSource, string sourceColumnName, string targetColumnName)
		{
			dtTarget.Columns.Add(targetColumnName);
			for (int i = 0; i < dtSource.Rows.Count; i++)
			{
				if (dtTarget.Rows.Count <= i)
				{
					dtTarget.Rows.Add(new object[0]);
				}
				dtTarget.Rows[i][targetColumnName] = dtSource.Rows[i][sourceColumnName];
			}
		}

		public static DataTable Reverse(this DataTable dt)
		{
			DataTable dataTable = new DataTable();
			for (int i = dt.Columns.Count - 1; i >= 0; i--)
			{
				dataTable.ImportColumn(dt, dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
			}
			return dataTable;
		}

		public static int ToInt(this object obj)
		{
			return Convert.ToInt32(obj);
		}

		public static T ToType<T>(this object obj)
		{
			T t;
			T t1;
			try
			{
				t = (T)Convert.ChangeType(obj, typeof(T));
			}
			catch
			{
				t1 = default(T);
				return t1;
			}
			t1 = t;
			return t1;
		}
	}
}