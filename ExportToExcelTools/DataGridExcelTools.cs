using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExportToExcelTools
{
	public static class DataGridExcelTools
	{
		public readonly static DependencyProperty IsExportedProperty;

		public readonly static DependencyProperty HeaderForExportProperty;

		public readonly static DependencyProperty PathForExportProperty;

		public readonly static DependencyProperty FormatForExportProperty;

		static DataGridExcelTools()
		{
			DataGridExcelTools.IsExportedProperty = DependencyProperty.RegisterAttached("IsExported", typeof(bool), typeof(DataGrid), new PropertyMetadata(true));
			DataGridExcelTools.HeaderForExportProperty = DependencyProperty.RegisterAttached("HeaderForExport", typeof(string), typeof(DataGrid), new PropertyMetadata(null));
			DataGridExcelTools.PathForExportProperty = DependencyProperty.RegisterAttached("PathForExport", typeof(string), typeof(DataGrid), new PropertyMetadata(null));
			DataGridExcelTools.FormatForExportProperty = DependencyProperty.RegisterAttached("FormatForExport", typeof(string), typeof(DataGrid), new PropertyMetadata(null));
		}

		public static void ExportToExcel(this DataGrid grid)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(DataGridExcelTools.StartExport));
			thread.Start(DataGridExcelTools.PrepareData(grid));
		}

		public static void ExportToExcel(this DataTable dt)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(DataGridExcelTools.StartExport));
			thread.Start(DataGridExcelTools.PrepareData(dt));
		}

		public static DataTable GetData(this DataGrid grid)
		{
			int i;
			DataTable dataTable = new DataTable();
			object[,] objArray = DataGridExcelTools.PrepareData(grid);
			int upperBound = objArray.GetUpperBound(0) + 1;
			int num = objArray.GetUpperBound(1) + 1;
			for (i = 0; i < num; i++)
			{
				object obj = objArray[0, i];
				if (dataTable.Columns.Contains(obj.ToString()))
				{
					obj = string.Concat(obj.ToString(), "_2");
				}
				dataTable.Columns.Add(obj.ToString());
			}
			for (int j = 1; j < upperBound; j++)
			{
				List<string> strs = new List<string>();
				for (i = 0; i < num; i++)
				{
					strs.Add(objArray[j, i].ToString());
				}
				dataTable.Rows.Add(strs.ToArray());
			}
			return dataTable;
		}

		public static string GetFormatForExport(DataGridColumn element)
		{
			return (string)element.GetValue(DataGridExcelTools.FormatForExportProperty);
		}

		private static string GetHeader(DataGridColumn column)
		{
			string headerForExport = DataGridExcelTools.GetHeaderForExport(column);
			return ((headerForExport != null ? true : column.Header == null) ? headerForExport : column.Header.ToString());
		}

		public static string GetHeaderForExport(DataGridColumn element)
		{
			return (string)element.GetValue(DataGridExcelTools.HeaderForExportProperty);
		}

		public static bool GetIsExported(DataGridColumn element)
		{
			return (bool)element.GetValue(DataGridExcelTools.IsExportedProperty);
		}

		private static string[] GetPath(DataGridColumn gridColumn)
		{
			string[] strArrays;
			string pathForExport = DataGridExcelTools.GetPathForExport(gridColumn);
			if (string.IsNullOrEmpty(pathForExport))
			{
				if (!(gridColumn is DataGridBoundColumn))
				{
					pathForExport = gridColumn.SortMemberPath;
				}
				else
				{
					Binding binding = ((DataGridBoundColumn)gridColumn).Binding as Binding;
					if (binding != null)
					{
						pathForExport = binding.Path.Path;
					}
				}
			}
			if (string.IsNullOrEmpty(pathForExport))
			{
				strArrays = null;
			}
			else
			{
				strArrays = pathForExport.Split(new char[] { '.' });
			}
			return strArrays;
		}

		public static string GetPathForExport(DataGridColumn element)
		{
			return (string)element.GetValue(DataGridExcelTools.PathForExportProperty);
		}

		private static object GetValue(string[] path, object obj, string formatForExport)
		{
			object obj1;
			string[] strArrays = path;
			int num = 0;
			while (true)
			{
				if (num < (int)strArrays.Length)
				{
					string str = strArrays[num];
					if (obj != null)
					{
						Type type = obj.GetType();
						PropertyInfo property = type.GetProperty(str);
						if (!(property == null))
						{
							obj = property.GetValue(obj, null);
							num++;
						}
						else
						{
							Debug.WriteLine(string.Format("Couldn't find property '{0}' in type '{1}'", str, type.Name));
							obj1 = null;
							break;
						}
					}
					else
					{
						obj1 = null;
						break;
					}
				}
				else if (string.IsNullOrEmpty(formatForExport))
				{
					obj1 = obj;
					break;
				}
				else
				{
					obj1 = string.Format(string.Concat("{0:", formatForExport, "}"), obj);
					break;
				}
			}
			return obj1;
		}

		public static object[,] PrepareData(DataTable dt)
		{
			object[,] columnName = new object[dt.Rows.Count + 1, dt.Columns.Count];
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				DataColumn item = dt.Columns[i];
				columnName[0, i] = item.ColumnName;
				for (int j = 1; j <= dt.Rows.Count; j++)
				{
					columnName[j, i] = dt.Rows[j - 1][i];
				}
			}
			return columnName;
		}

		public static object[,] PrepareData(DataGrid grid)
		{
			List<DataGridColumn> list = (
				from x in grid.Columns
				where (!DataGridExcelTools.GetIsExported(x) ? false : (x is DataGridBoundColumn || !string.IsNullOrEmpty(DataGridExcelTools.GetPathForExport(x)) ? true : !string.IsNullOrEmpty(x.SortMemberPath)))
				select x).ToList<DataGridColumn>();
			List<object> objs = grid.ItemsSource.Cast<object>().ToList<object>();
			object[,] header = new object[objs.Count + 1, list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				DataGridColumn item = list[i];
				header[0, i] = DataGridExcelTools.GetHeader(item);
				string[] path = DataGridExcelTools.GetPath(item);
				string formatForExport = DataGridExcelTools.GetFormatForExport(item);
				if (path != null)
				{
					for (int j = 1; j <= objs.Count; j++)
					{
						object obj = objs[j - 1];
						header[j, i] = DataGridExcelTools.GetValue(path, obj, formatForExport);
					}
				}
			}
			return header;
		}

		public static void SetFormatForExport(DataGridColumn element, string value)
		{
			element.SetValue(DataGridExcelTools.FormatForExportProperty, value);
		}

		public static void SetHeaderForExport(DataGridColumn element, string value)
		{
			element.SetValue(DataGridExcelTools.HeaderForExportProperty, value);
		}

		public static void SetIsExported(DataGridColumn element, bool value)
		{
			element.SetValue(DataGridExcelTools.IsExportedProperty, value);
		}

		public static void SetPathForExport(DataGridColumn element, string value)
		{
			element.SetValue(DataGridExcelTools.PathForExportProperty, value);
		}

		private static void StartExport(object data)
		{
			ExportManager.ExportToExcel(data as object[,]);
		}
	}
}