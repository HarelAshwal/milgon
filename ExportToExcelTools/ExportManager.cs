using Microsoft.CSharp.RuntimeBinder;
using Microsoft.VisualBasic;
using System;
using System.Runtime.CompilerServices;

namespace ExportToExcelTools
{
	public static class ExportManager
	{
		public static void ExportToExcel(object[,] data)
		{
			dynamic obj = Interaction.CreateObject("Excel.Application", string.Empty);
			obj.ScreenUpdating = false;
			dynamic obj1 = obj.workbooks;
			obj1.Add();
			dynamic obj2 = obj.ActiveSheet;
			int length = data.GetLength(0);
			int num = data.GetLength(1);
			int num1 = 1 + length - 1;
			int num2 = 1 + num - 1;
			if ((length == 0 ? false : num != 0))
			{
				dynamic obj3 = obj2.Range[obj2.Cells[1, 1], obj2.Cells[num1, num2]];
				obj3.Value = data;
				for (int i = 1; i <= 4; i++)
				{
					obj3.Borders[i].LineStyle = 1;
				}
				obj3.EntireColumn.AutoFit();
				dynamic obj4 = obj2.Range[obj2.Cells[1, 1], obj2.Cells[1, num2]];
				obj4.Font.Bold = true;
				obj.ScreenUpdating = true;
				obj.Visible = true;
			
				obj3 = null;
				obj4 = null;
				obj2 = null;
				obj1 = null;
				obj = null;
			}
		}
	}
}