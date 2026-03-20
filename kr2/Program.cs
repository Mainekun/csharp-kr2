using kr2.Roles;
using System.Net;
using System.Runtime.ExceptionServices;

namespace kr2
{
    internal class Program
    {
		static List<Room> rooms = new List<Room>();
		static List<Client> clients = new List<Client>();
		static List<StaffMember> staffMembers = new List<StaffMember>();
		static List<KeyValuePair<int, int>> assignedFloors = new List<KeyValuePair<int, int>>();
		static List<KeyValuePair<int, Weekday>> schedule = new List<KeyValuePair<int, Weekday>>();

		static readonly int TABLE_COUNT = 5; 

		//static string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        static String[] path = { "\\Tables\\Rooms.txt", "\\Tables\\Clients.txt" , "\\Tables\\Staff.txt", "\\Tables\\AssignedFloors.txt", "\\Tables\\Schedule.txt" };
		static int[] m = { 4, 8, 2, 2, 2 };

        static void Main(string[] args)
        {
			LoadTablesFromText();

			bool mainExitFlag = false;
			

			while (mainExitFlag == false)
			{
				InterfacePrinter.PrintMainMenu();
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);
				InterfacePrinter.ClearConsole();

				switch (pressedKey.Key)
				{
					case ConsoleKey.D1:
						PrintMenuLoop();
						break;
					case ConsoleKey.D2:
						QueriesMenuLoop();
						break;
					case ConsoleKey.D3:
						EditStaffMenuLoop();
						break;
					case ConsoleKey.D0:
						mainExitFlag = true;
						Console.WriteLine("Exit");
						break;
				}
			}
        }

		static void PrintMenuLoop()
		{
			InterfacePrinter.PrintPrintMenu();
			bool exitFlag = false;
			while(!exitFlag)
			{
				
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);
				InterfacePrinter.ClearPrintMenuOutput();

				switch(pressedKey.Key)
				{
					case ConsoleKey.D1:
						PrintRooms();
						break;
					case ConsoleKey.D2:
						PrintClients();
						break;
					case ConsoleKey.D3:
						PrintStaff();
						break;
					case ConsoleKey.D4:
						PrintAssignedFloors();
						break;
					case ConsoleKey.D5:
						PrintSchedule();
						break;
					case ConsoleKey.D0:
						exitFlag = true;
						InterfacePrinter.ClearConsole();
						break;
				}
			}
		}

		static void QueriesMenuLoop()
		{
			
			bool exitFlag = false;
			while (!exitFlag)
			{
				InterfacePrinter.PrintQueriesMenu();
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);

				switch (pressedKey.Key)
				{
					case ConsoleKey.D0:
						exitFlag = true;
						InterfacePrinter.ClearConsole();
						break;
				}
			}
		}

		static void EditStaffMenuLoop()
		{
			
			bool exitFlag = false;
			while (!exitFlag)
			{
				InterfacePrinter.PrintStaffMenu();
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);

				switch (pressedKey.Key)
				{
					case ConsoleKey.D0:
						exitFlag = true;
						InterfacePrinter.ClearConsole();
						break;
				}
			}
		}

		static void LoadTablesFromText()
		{
			for (int i = 0; i < TABLE_COUNT; i++)
			{
				try
				{
					using (StreamReader sr = new StreamReader(path[i]))
					{
						string line;
						while ((line = sr.ReadLine()) != null)
						{
							string[] ms = new string[m[i]];
							ms = line.Split(';');

							switch (i)
							{
								case 0:
									if (int.TryParse(ms[0], out int number) &&
										int.TryParse(ms[1], out int floor) &&
										int.TryParse(ms[2], out int type) &&
										int.TryParse(ms[3], out int cost))
									{
										rooms.Add(new Room(number, floor, (RoomType)type, cost));
									}
									else
									{
										Console.WriteLine($"[Rooms.txt]: incorrect line: \"{line}\"");
									}
									break;

								case 1:
									if (int.TryParse(ms[0], out int clientKey) &&
										int.TryParse(ms[4], out int paidNumber) &&
										int.TryParse(ms[5], out int place) &&
										DateOnly.TryParseExact(ms[6], "dd/MM/yyyy", out DateOnly arrivalDate) &&
										int.TryParse(ms[7], out int paidDays))
									{
										clients.Add(new Client(clientKey, ms[1], ms[2], ms[3], paidNumber, place, arrivalDate, paidDays));
									}
									else
									{
										Console.WriteLine($"[Clients.txt]: incorrect line: \"{line}\"");
									}
									break;

								case 2:
									if (int.TryParse(ms[0], out int staffKey))
									{
										staffMembers.Add(new StaffMember(staffKey, ms[1]));
									}
									else
									{
										Console.WriteLine($"[Staff.txt]: incorrect line: \"{line}\"");
									}
									break;

								case 3:
									if (int.TryParse(ms[0], out int staffToFloorKey) &&
										int.TryParse(ms[1], out int assignedFloor))
									{
										assignedFloors.Add(new KeyValuePair<int, int>(staffToFloorKey, assignedFloor));
									}
									else
									{
										Console.WriteLine($"[AssignedFloors.txt]: incorrect line: \"{line}\"");
									}

									break;

								case 4:
									if (int.TryParse(ms[0], out int staffToWeekdayKey) &&
										int.TryParse(ms[1], out int weekday))
									{
										assignedFloors.Add(new KeyValuePair<int, int>(staffToWeekdayKey, weekday));
									}
									else
									{
										Console.WriteLine($"[Schedule.txt]: incorrect line: \"{line}\"");
									}

									break;
							}
						}
					}

				}
				catch (DirectoryNotFoundException ex) 
				{ 
					Console.WriteLine(ex.Message); 
				}
				catch (FileNotFoundException ex)
				{
					Console.WriteLine(ex.Message);
				}


			}
		} //LoadTablesFromFile


		static void PrintRooms()
		{
			foreach (Room room in rooms) 
			{ 
				Console.Write("-> " + room.ToLineString()); 
			}
		}

		static void PrintClients()
		{
			foreach (Client client in clients) 
			{ 
				Console.Write("-> " + client.ToLineString()); 
			}
		}

		static void PrintStaff()
		{
			foreach (StaffMember staffMember in staffMembers) 
			{ 
				Console.Write("-> " + staffMember.ToLineString()); 
			}
		}

		static void PrintAssignedFloors()
		{
			foreach (KeyValuePair<int, int> pair in assignedFloors)
			{
				Console.WriteLine($"-> {pair.Key};{pair.Value}");
			}
		}

		static void PrintSchedule()
		{
			foreach (KeyValuePair <int, Weekday> pair in schedule)
			{
				Console.WriteLine($"-> {pair.Key};{pair.Value.ToString()}");
			}
		}
    }
}
