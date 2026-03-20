using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace kr2
{
    internal class InterfacePrinter
    {
		public static void ClearConsole()
		{
			Console.Write("\x1b[2J\x1b[H");
		}

		public static void PrintMainMenu()
		{
			Console.WriteLine(
				"1.Print\n" +
				"2.Queries\n" +
				"3.Edit staff\n" +
				"\n" +
				"0.Quit\n");
		}

		public static void PrintPrintMenu()
		{
			Console.WriteLine(
				"Print\n" +
				"1.Rooms\n" +
				"2.Clients\n" +
				"3.Staff\n" +
				"4.Floors\n" +
				"5.Schedule\n" +
				"\n" +
				"0.Quit\n");
		}

		public static void ClearPrintMenuOutput()
		{
			Console.Write("\x1b[H\x1b[9B\x1b[0J");
		}

		public static void PrintQueriesMenu()
		{
			Console.WriteLine(
				"Queries\n" +
				"1.Cost of sertain room\n" +
				"2.List of client with certain whence\n" +
				"3.Who cleaned client's room in certain weekday\n" +
				"4.Available rooms and places\n" +
				"5.Occupied singlerooms\n" +
				"6.Total gain\n" +
				"\n" +
				"0.Quit\n");
		}

		public static void ClearQueriesMenuOutput()
		{
			Console.Write("\x1b[H\x1b[10B\x1b[0J");
		}

		public static void PrintStaffMenu()
		{
			Console.WriteLine(
				"Staff\n" +
				"1.Print staff\n" +
				"2.Hire new one\n" +
				"3.Fire old one\n" +
				"\n" +
				"0.Quit\n");
		}

		public static KeyValuePair<int, int> ReadRoomFromConsole()
		{
			String str = Console.ReadLine();
			if (str == null)
			{
				return new KeyValuePair<int, int>(-1, -1);
			}
			String[] room = str.Split(' ');
			if (int.TryParse(room[0], out int roomNumber) &&
				int.TryParse(room[1], out int floor))
			{
				return new KeyValuePair<int, int>(roomNumber, floor);
			}
			return new KeyValuePair<int, int>(-1, -1);
		}
	}
}
