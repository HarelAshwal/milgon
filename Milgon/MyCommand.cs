using System;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace Milgon
{
	public class MyCommand
	{
		public bool CanExecute
		{
			get;
			set;
		}

		public RoutedUICommand command
		{
			get;
			private set;
		}

		public CommandBinding commandBinding
		{
			get;
			set;
		}

		public MyCommand(string Name, ExecutedRoutedEventHandler executedRoutedEventHandler, MenuItem menuItem, bool DefaultCanExecute = false)
		{
			this.CanExecute = DefaultCanExecute;
			this.command = new RoutedUICommand(Name, Name, typeof(MainWindow));
			this.commandBinding = new CommandBinding(this.command);
			this.commandBinding.CanExecute += new CanExecuteRoutedEventHandler(this.commandBinding_CanExecute);
			this.commandBinding.Executed += executedRoutedEventHandler;
			menuItem.Command = this.command;
		}

		private void commandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.CanExecute;
		}
	}
}