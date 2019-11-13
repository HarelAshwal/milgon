using Milgon;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Milgon.Windows
{
	public partial class Win_InsertData : UserControl
	{
		private GlobalAvrechEntranceData data = new GlobalAvrechEntranceData();

		public Win_InsertData(GlobalAvrechEntranceData dataInstance)
		{
			this.InitializeComponent();
			this.data = dataInstance;
		}

		private void AddColumn(string Header1, string Header2, string Header3, string BindingPath)
		{
			StackPanel stackPanel = new StackPanel();
			TextBlock textBlock = new TextBlock();
			TextBlock header2 = new TextBlock();
			TextBlock header3 = new TextBlock();
			stackPanel.Children.Add(textBlock);
			stackPanel.Children.Add(header2);
			stackPanel.Children.Add(header3);
			textBlock.Text = Header1;
			header2.Text = Header2;
			header3.Text = Header3;
			Binding binding = new Binding(BindingPath)
			{
				StringFormat = "HH:mm"
			};
			DataGridTextColumn dataGridTextColumn = new DataGridTextColumn()
			{
				Binding = binding,
				Header = stackPanel
			};
			this.dataGrid_data.Columns.Add(dataGridTextColumn);
		}

		private GlobalAvrechEntranceData GenerateAvrechData()
		{
			GlobalAvrechEntranceData globalAvrechEntranceDatum = new GlobalAvrechEntranceData();
			AvrechMonthData avrechMonthDatum = new AvrechMonthData(new int?(0), "user0");
			avrechMonthDatum.AddGlobalAdditions(10, 10, 0, 10, 20, 30, 10, 0, false,0,0,0);
			avrechMonthDatum.AddDayEntry(new EnteranceRecord(DateTime.Now, false), new EnteranceRecord(DateTime.Now, false), new EnteranceRecord(DateTime.Now, false), new EnteranceRecord(DateTime.Now, false), false, false);
			globalAvrechEntranceDatum.AllAvrechData.Add(avrechMonthDatum);
			return globalAvrechEntranceDatum;
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			this.data = this.GenerateAvrechData();
			this.dataGrid_data.ItemsSource = this.data.AllAvrechData;
			int num = 0;
			foreach (EnteranceDataDay day in this.data.AllAvrechData[0].entranceDataMonth.Days)
			{
				DateTime dayDate = day.DayDate;
				this.AddColumn(string.Concat("יום '", dayDate.DayOfWeek), "סדר א'", "כניסה", string.Format("entranceDataMonth.Days[{0}].SederA.In", num));
				this.AddColumn("", "", "יציאה", string.Format("entranceDataMonth.Days[{0}].SederA.Out", num));
				this.AddColumn("", "סדר ב'", "כניסה", string.Format("entranceDataMonth.Days[{0}].SederB.In", num));
				this.AddColumn("", "", "יציאה", string.Format("entranceDataMonth.Days[{0}].SederB.Out", num));
				num++;
			}
		}
	}
}