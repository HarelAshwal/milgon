using Microsoft.Win32;
using Milgon.Windows;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using WPF.MDI;

namespace Milgon
{
	public partial class MainWindow : Window
	{
		private const string MILGON_DATA_FILE_EXTENSION = ".milgon";

		private MyCommand cmdReportsAllMilgot;

		private MyCommand cmdReportsSummaryMilgot;

		private MyCommand cmdReportsSpecificMilga;

		private MyCommand cmdProperties;

		private MyCommand cmdInsertData;

		private GlobalAvrechEntranceData data = new GlobalAvrechEntranceData();

		private bool Save_CanExecute = false;

		private DataContractSerializer dcs = new DataContractSerializer(typeof(GlobalAvrechEntranceData));

		public MainWindow()
		{
			this.InitializeComponent();
		}

		private void cbDataInsert_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!this.WindowExists(typeof(Win_InsertData)))
			{
				ObservableCollection<MdiChild> children = this.Container.Children;
				MdiChild mdiChild = new MdiChild()
				{
					Title = "הזנת נתונים",
					Content = new Win_InsertData(this.data),
					Width = 750,
					Height = 350
				};
				children.Add(mdiChild);
			}
			this.cmdReportsSummaryMilgot.CanExecute = true;
			this.cmdReportsAllMilgot.CanExecute = true;
			this.cmdReportsSpecificMilga.CanExecute = true;
			this.Save_CanExecute = true;
		}

		private void cbProperties_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!this.WindowExists(typeof(Win_Properties)))
			{
				ObservableCollection<MdiChild> children = this.Container.Children;
				MdiChild mdiChild = new MdiChild()
				{
					Title = "הגדרות מלגה",
					Content = new Win_Properties(),
					Width = 750,
					Height = 350
				};
				children.Add(mdiChild);
			}
		}

		private void cbReportsAllMilgot_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!this.WindowExists(typeof(Win_MilgotReport)))
			{
				ObservableCollection<MdiChild> children = this.Container.Children;
				MdiChild mdiChild = new MdiChild()
				{
					Title = "דוח - כל המלגות",
					Content = new Win_MilgotReport(this.data, this.Container),
					Width = 750,
					Height = 350
				};
				children.Add(mdiChild);
			}
		}

		private void cbReportsSummaryMilgot_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (!this.WindowExists(typeof(Win_MilgonSummaryReport)))
			{
				ObservableCollection<MdiChild> children = this.Container.Children;
				MdiChild mdiChild = new MdiChild()
				{
					Title = "דוח - מלגות מקוצר",
					Content = new Win_MilgonSummaryReport(this.data, this.Container),
					Width = 750,
					Height = 350
				};
				children.Add(mdiChild);
			}
		}

		private void cbSave_Executed(object sender, ExecutedRoutedEventArgs e)
		{
		}

		private void CommandBinding_Close_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			base.Close();
		}

		private void CommandBinding_Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				CheckFileExists = true,
				Filter = "Milgon Data files |*.xlsx;*.milgon"
			};
			bool? nullable = openFileDialog.ShowDialog();
			if (nullable.Value)
			{
				FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
				this.StatusBar_FileName.Content = fileInfo.Name;
				try
				{
					if (!(fileInfo.Extension == ".milgon"))
					{
						DataTable dataTable = CommonLibrary.LoadExcelFile(fileInfo.FullName);
						dataTable.Reverse();
						this.data = new GlobalAvrechEntranceData();
						this.data.LoadFromDataTable(dataTable);
					}
					else
					{
						this.data = new GlobalAvrechEntranceData();
						this.data.LoadData(fileInfo.FullName);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					MessageBox.Show(string.Concat("Error : ", exception.Message), "System Message");
				}
				this.cmdReportsSummaryMilgot.CanExecute = true;
				this.cmdReportsAllMilgot.CanExecute = true;
				this.cmdReportsSpecificMilga.CanExecute = true;
				this.Save_CanExecute = true;
			}
		}

		private void CommandBinding_Print_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.WindowExists(typeof(Win_MilgotReport));
		}

		private void CommandBinding_Print_Executed(object sender, ExecutedRoutedEventArgs e)
		{
		}

		private void CommandBinding_Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.Save_CanExecute;
		}

		private void CommandBinding_Save_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (this.data != null)
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog()
				{
					DefaultExt = ".milgon"
				};
				saveFileDialog.ShowDialog();
				this.data.SaveData(saveFileDialog.FileName);
			}
		}

		private void InvokeError(string Message)
		{
			MessageBox.Show(Message, "שגיאה");
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.Container.Theme = ThemeType.Aero;
			this.StatusBar_Version.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
			this.StatusBar_FileName.Content = "";
			this.cmdProperties = new MyCommand("Properties", new ExecutedRoutedEventHandler(this.cbProperties_Executed), this.menuProperties, true);
			this.cmdReportsAllMilgot = new MyCommand("Reports.AllMilgot", new ExecutedRoutedEventHandler(this.cbReportsAllMilgot_Executed), this.menuReportsAllMilgot, false);
			this.cmdReportsSummaryMilgot = new MyCommand("Reports.SummaryMilgot", new ExecutedRoutedEventHandler(this.cbReportsSummaryMilgot_Executed), this.menuReportsSummaryMilgot, false);
			this.cmdReportsSpecificMilga = new MyCommand("Reports.SpecificMilga", new ExecutedRoutedEventHandler(this.cbReportsAllMilgot_Executed), this.menuReportsSpecificMilga, false);
			this.cmdInsertData = new MyCommand("Data.InsertData", new ExecutedRoutedEventHandler(this.cbDataInsert_Executed), this.menuInsertData, true);
			base.CommandBindings.Add(this.cmdProperties.commandBinding);
			base.CommandBindings.Add(this.cmdReportsAllMilgot.commandBinding);
			base.CommandBindings.Add(this.cmdReportsSummaryMilgot.commandBinding);
			base.CommandBindings.Add(this.cmdReportsSpecificMilga.commandBinding);
			base.CommandBindings.Add(this.cmdInsertData.commandBinding);
		}

		private bool WindowExists(Type window)
		{
			IEnumerable<MdiChild> children = 
				from MdiWindow in this.Container.Children
				where MdiWindow.Content.GetType() == window
				select MdiWindow;
			return children.Count<MdiChild>() > 0;
		}
	}
}