using System;
using System.Windows.Input;

namespace Milgon
{
	public static class Commands
	{
		public static RoutedUICommand MenuOpen;

		static Commands()
		{
			Commands.MenuOpen = new RoutedUICommand("Menu.Open", "Menu.Open", typeof(MainWindow));
		}
	}
}