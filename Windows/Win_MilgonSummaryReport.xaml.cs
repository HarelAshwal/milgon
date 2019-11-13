using ExportToExcelTools;
using Milgon;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WPF.MDI;

namespace Milgon.Windows
{
	public partial class Win_MilgonSummaryReport : UserControl
	{
		private MdiContainer Container;

		private GlobalAvrechEntranceData AvrechData
		{
			get;
			set;
		}

		public Win_MilgonSummaryReport(GlobalAvrechEntranceData AvrechData, MdiContainer Container)
		{
			this.InitializeComponent();
			this.Container = Container;
			this.AvrechData = AvrechData;
		}

		private void button_Print_Click(object sender, RoutedEventArgs e)
		{
			this.dataGrid_Report.GetData().ExportToExcel();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			string[] strArrays = new string[] { "כל החודש", "שבועי", "חצי חודש" };
			this.comboBox_Ranges.ItemsSource = strArrays;
			this.dataGrid_Report.ItemsSource = this.AvrechData.AllAvrechData;
		}
	}
}