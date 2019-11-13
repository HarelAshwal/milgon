using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using Milgon;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Milgon.Windows
{
	public partial class Win_Properties : UserControl
	{
		private DataContractSerializer dcs = new DataContractSerializer(typeof(MilgaStructure));

		public Win_Properties()
		{
			this.InitializeComponent();
		}

		private void button_Save_Click(object sender, RoutedEventArgs e)
		{
			MilgaStructure milgaStructure = new MilgaStructure()
			{
				BasicMilga = this.doubleUpDown_BaseMilga.Value,
				SummaryBonus = this.doubleUpDown_SummaryBonus.Value,
				SederA = new Seder()
				{
					Bonus = this.doubleUpDown_SederA_Bonus.Value,
					LateCountForBonusCanceling = this.integerUpDown_SederA_LateCountForBonusCanceling.Value,
					StartTime = this.dateTimeUpDown_SederA_StartTime.Value.ClearSeconds(),
					StartTimeBonus = this.dateTimeUpDown_SederA_StartTimeBonus.Value.ClearSeconds(),
					EndTime = this.dateTimeUpDown_SederA_EndTime.Value.ClearSeconds()
				},
				SederB = new Seder()
				{
					Bonus = this.doubleUpDown_SederB_Bonus.Value,
					LateCountForBonusCanceling = this.integerUpDown_SederB_LateCountForBonusCanceling.Value,
					StartTime = this.dateTimeUpDown_SederB_StartTime.Value.ClearSeconds(),
					StartTimeBonus = this.dateTimeUpDown_SederB_StartTimeBonus.Value.ClearSeconds(),
					EndTime = this.dateTimeUpDown_SederB_EndTime.Value.ClearSeconds()
				},
				MissingHourFine = this.doubleUpDown_MissingHourFine.Value,
				KolelShishiMilga = this.doubleUpDown_KolelShishiMilga.Value
			};
			CommonLibrary.SavemilgaStructure(milgaStructure);
			System.Windows.MessageBox.Show("!נשמר בהצלחה", "הודעת מערכת");
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			MilgaStructure commonmilgaStructure = CommonLibrary.CommonmilgaStructure;
			this.doubleUpDown_BaseMilga.Value = commonmilgaStructure.BasicMilga;
			this.doubleUpDown_SummaryBonus.Value = commonmilgaStructure.SummaryBonus;
			this.doubleUpDown_SederA_Bonus.Value = commonmilgaStructure.SederA.Bonus;
			this.integerUpDown_SederA_LateCountForBonusCanceling.Value = commonmilgaStructure.SederA.LateCountForBonusCanceling;
			this.dateTimeUpDown_SederA_StartTime.Value = commonmilgaStructure.SederA.StartTime;
			this.dateTimeUpDown_SederA_StartTimeBonus.Value = commonmilgaStructure.SederA.StartTimeBonus;
			this.dateTimeUpDown_SederA_EndTime.Value = commonmilgaStructure.SederA.EndTime;
			this.doubleUpDown_SederB_Bonus.Value = commonmilgaStructure.SederB.Bonus;
			this.integerUpDown_SederB_LateCountForBonusCanceling.Value = commonmilgaStructure.SederB.LateCountForBonusCanceling;
			this.dateTimeUpDown_SederB_StartTime.Value = commonmilgaStructure.SederB.StartTime;
			this.dateTimeUpDown_SederB_StartTimeBonus.Value = commonmilgaStructure.SederB.StartTimeBonus;
			this.dateTimeUpDown_SederB_EndTime.Value = commonmilgaStructure.SederB.EndTime;
			this.doubleUpDown_MissingHourFine.Value = commonmilgaStructure.MissingHourFine;
			this.doubleUpDown_KolelShishiMilga.Value = commonmilgaStructure.KolelShishiMilga;
		}
	}
}