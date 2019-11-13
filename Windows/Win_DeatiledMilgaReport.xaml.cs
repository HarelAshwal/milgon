using ExportToExcelTools;
using Milgon;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Milgon.Windows
{
	public partial class Win_DeatiledMilgaReport : UserControl
	{
		private AvrechMonthData avrechMonthData = null;

		public Win_DeatiledMilgaReport(AvrechMonthData avrechMonthData)
		{
			this.InitializeComponent();
			this.avrechMonthData = avrechMonthData;
		}

		private void button_Print_Click(object sender, RoutedEventArgs e)
		{
			DataTable data = this.dataGrid_DeatiledReport.GetData();
			for (int i = 0; i < data.Rows.Count; i++)
			{
				for (int j = 0; j < (int)data.Rows[i].ItemArray.Length; j++)
				{
					data.Rows[i][j] = data.Rows[i][j].ToString().Replace("True", "√").Replace("False", "");
				}
			}
			data.ExportToExcel();
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.dataGrid_DeatiledReport.ItemsSource = this.avrechMonthData.entranceDataMonth.Days;
		}
	}
}