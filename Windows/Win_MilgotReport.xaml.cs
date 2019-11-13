using ExportToExcelTools;
using Milgon;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using WPF.MDI;

namespace Milgon.Windows
{
	public partial class Win_MilgotReport : UserControl
	{
		private MdiContainer Container;

		private bool isManualEditCommit = false;

		private GlobalAvrechEntranceData AvrechData
		{
			get;
			set;
		}

    private MilgaStructure milgaStracture
    {
      get;set;

    }

		public Win_MilgotReport(GlobalAvrechEntranceData AvrechData, MdiContainer Container)
		{
			this.InitializeComponent();
			this.Container = Container;
			this.AvrechData = AvrechData;
      this.milgaStracture = CommonLibrary.CommonmilgaStructure;
      

    }


    private DataTable RoundTable(DataTable data)
    {
      // round digits
      for (var i = 0; i < data.Columns.Count; i++)
      {
        for (var j = 0; j < data.Rows.Count; j++)
        {
          var val = data.Rows[j][i].ToString();

          double n;
          bool isNumeric = double.TryParse(val, out n);

          if (isNumeric)
          {
            val = n.ToString("F1");
            data.Rows[j][i] = val;
          }
        }
      }

      return data;
    }

		private void button_Print_Click(object sender, RoutedEventArgs e)
		{
      // export table 1
      DataTable data = PrintTotal(true);

      // export table 2
      PrintPresence();

      // export table 3
      PrintBonos();

    }

    private int CloneColumn(ref DataTable dt, string ColumnName)
		{
			int i;
			string columnName = ColumnName;
			for (i = 0; i < 10; i++)
			{
				if (dt.Columns.Contains(columnName))
				{
					columnName = string.Concat(columnName, " ");
				}
			}
			dt.Columns.Add(columnName);
			for (i = 0; i < dt.Rows.Count; i++)
			{
				dt.Rows[i][columnName] = dt.Rows[i][ColumnName];
			}
			return dt.Columns.Count - 1;
		}

		private void dataGrid_Report_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			if (!this.isManualEditCommit)
			{
				this.isManualEditCommit = true;
				DataGrid dataGrid = (DataGrid)sender;
				dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
				this.isManualEditCommit = false;
				dataGrid.Items.Refresh();
			}
		}

		private void dataGrid_Report_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.dataGrid_Report.SelectedItem != null)
			{
				AvrechMonthData selectedItem = (AvrechMonthData)this.dataGrid_Report.SelectedItem;
				ObservableCollection<MdiChild> children = this.Container.Children;
				MdiChild mdiChild = new MdiChild()
				{
					Title = string.Concat(" דוח - מפורט ", selectedItem.Name),
					Content = new Win_DeatiledMilgaReport(selectedItem),
					Width = 750,
					Height = 350
				};
				children.Add(mdiChild);
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.dataGrid_Report.ItemsSource = this.AvrechData.AllAvrechData;
		}

    private void button_PrintBonos_Click(object sender, RoutedEventArgs e)
    {
      PrintBonos();
    }

    private void PrintBonos()
    {
      DataTable data3 = this.dataGrid_Report.GetData();
      data3.Columns["ימי חיסור א"].SetOrdinal(2);
      data3.Columns["איחורים א"].SetOrdinal(3);
      data3.Columns["הפסקות א"].SetOrdinal(4);
      data3.Columns["חסרון בונוס א"].SetOrdinal(5);
      data3.Columns["אישורים א"].SetOrdinal(6);
      data3.Columns["זכאות בונוס א"].SetOrdinal(7);
      data3.Columns["בונוס א"].SetOrdinal(8);
      data3.Columns.Add("       ");


      int num = this.CloneColumn(ref data3, "מספר");
      data3.Columns[num].SetOrdinal(10);

      num = this.CloneColumn(ref data3, "שם");
      data3.Columns[num].SetOrdinal(11);

      data3.Columns["ימי חיסור ב"].SetOrdinal(12);
      data3.Columns["איחורים ב"].SetOrdinal(13);
      data3.Columns["הפסקות ב"].SetOrdinal(14);
      data3.Columns["חסרון בונוס ב"].SetOrdinal(15);
      data3.Columns["אישורים ב"].SetOrdinal(16);
      data3.Columns["זכאות בונוס ב"].SetOrdinal(17);
      data3.Columns["בונוס ב"].SetOrdinal(18);


      var TotalColCount = data3.Columns.Count;
      var ColToDelete = TotalColCount - 19;
      for (var i = 0; i < ColToDelete; i++) data3.Columns.RemoveAt(19);

      data3.Columns[9].ColumnName = "  -   ";
      for (var i = 0; i < data3.Rows.Count; i++) data3.Rows[i][9] = "";
      data3 = RoundTable(data3);
      data3.ExportToExcel();
    }

    private void button_PrintTotal_Click(object sender, RoutedEventArgs e)
    {
      PrintTotal();
    }

    private DataTable PrintTotal(Boolean IsPrint = true)
    {
      // export table 1
      DataTable data = this.dataGrid_Report.GetData();

      data = RoundTable(data);

      data.Columns.Add(new DataColumn("בסיס"));
      var colCount = data.Rows.Count;

      for (var i = 0; i < colCount; i++)
      {
        data.Rows[i]["בסיס"] = CommonLibrary.CommonmilgaStructure.BasicMilga;
      }

      data.Columns["בסיס"].SetOrdinal(2);

      var TotalColCount = data.Columns.Count;
      var ColToDelete = TotalColCount - 10;
      for (var i = 0; i < ColToDelete; i++) data.Columns.RemoveAt(10);

      if (IsPrint) data.ExportToExcel();

      return data;
    }

    private void button_Print_PresenceClick(object sender, RoutedEventArgs e)
    {
      PrintPresence();
    }

    private void PrintPresence()
    {
      DataTable data2 = this.dataGrid_Report.GetData();

      data2.Columns["סה\"כ שעות"].SetOrdinal(2);
      data2.Columns["חסרון שעות"].SetOrdinal(3);
      data2.Columns["אחוז חסרון"].SetOrdinal(4);
      data2.Columns["שעות נוכחות"].SetOrdinal(5);
      data2.Columns["אחוז נוכחות"].SetOrdinal(6);
      data2.Columns["שעות מאושרות"].SetOrdinal(7);

      var TotalColCount = data2.Columns.Count;
      var ColToDelete = TotalColCount - 8;
      for (var i = 0; i < ColToDelete; i++) data2.Columns.RemoveAt(8);

      data2 = RoundTable(data2);

      data2.ExportToExcel();
    }

    private void button_PrintSummary_Click(object sender, RoutedEventArgs e)
    {
      DataTable data2 = this.dataGrid_Report.GetData();

      data2.Columns["חבורות"].SetOrdinal(2);
      data2.Columns["סיכומים"].SetOrdinal(3);
      data2.Columns["מבחנים"].SetOrdinal(4);

      var TotalColCount = data2.Columns.Count;
      var ColToDelete = TotalColCount - 5;
      for (var i = 0; i < ColToDelete; i++) data2.Columns.RemoveAt(5);

      data2 = RoundTable(data2);

      data2.ExportToExcel();


    }
  }
}