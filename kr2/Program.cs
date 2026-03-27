using kr2.Roles;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

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
                    case ConsoleKey.Escape:
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
                    case ConsoleKey.Escape:
                        exitFlag = true;
						InterfacePrinter.ClearConsole();
						break;
				}
			}
		}

		static void QueriesMenuLoop()
		{
            InterfacePrinter.PrintQueriesMenu();
            bool exitFlag = false;
			while (!exitFlag)
			{
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);
				InterfacePrinter.ClearQueriesMenuOutput();

				switch (pressedKey.Key)
				{
						case ConsoleKey.D1:
						{
							var room = InterfacePrinter.ReadRoomFromConsole();
							var roomInfo = rooms
								.Where(r => r.Number == room.Key && r.Floor == room.Value)
								.Select(r => r.Cost);
							if (roomInfo.ToList().Count == 0)
							{
								Console.WriteLine("There is no such room");
								break;
							}
							var cost = roomInfo.First();
								Console.WriteLine($"Cost of room {room.Key} on {room.Value} floor costs {cost}$");
						}
						break;
					case ConsoleKey.D2:
						{
							string whence = Console.ReadLine();
							List<Client> filteredClients = clients.Where(c => c.Whence == whence).ToList();
							foreach (Client client in filteredClients)
							{
								Console.WriteLine(client.Name);
							}
						}
						break;
					case ConsoleKey.D3:
						{
							string clientsName = Console.ReadLine();
							string strWeekday = Console.ReadLine();
							Weekday weekday;
							if (int.TryParse(strWeekday, out int day))
							{
								if (day >= 0 && day <= 6)
								{
									weekday = (Weekday)day;
								}
								else
								{
									Console.WriteLine("Weekday must be in range from 0 to 6, Monday to Sunday accordingly");
									break;
								}
							}
							else
							{
								Console.WriteLine("Weekday must be integer");
								break;
							}
							var clientsRoom = clients
								.Where(c => c.Name == clientsName)
								.Select(c => c.RoomNumber)
								.ToList();
							if (clientsRoom.Count < 1)
							{
								Console.WriteLine("There is no such person in hotel");
								break;
							}
							var targetFloor = rooms
								.Where(r => r.Number == clientsRoom.First())
								.Select(r => r.Floor)
								.ToList();
							if (targetFloor.Count < 1)
							{
								Console.WriteLine("There is no such room");
								break;
							}
							var staffOnFloor = assignedFloors
								.Where(p => p.Value == targetFloor.First())
								.ToList();
							var staffOnDay = schedule
								.Where(p => p.Value == weekday)
								.ToList();
							var staffOnDayAndFloor = staffOnDay
								.Join(
									staffOnFloor,
									d => d.Key,
									f => f.Key,
									(d, f) => new { Key = d.Key })
								.Select(s => s.Key)
								.ToList();
							var targetStaff = staffMembers
								.Where(s => staffOnDayAndFloor.Contains(s.Key))
								.ToList();

							Console.WriteLine($"Found {targetStaff.Count} staffmembers");
							foreach (StaffMember staff in targetStaff)
							{
								Console.WriteLine(staff.FullName);
							}
						}
						break;
					case ConsoleKey.D4:
						{
							var availableRooms = rooms
							.LeftJoin(
								clients,
								r => r.Number,
								c => c.RoomNumber,
								(r, c) => new {
									P = c?.Place,
									Number = r.Number,
									RType = r.Type})
							.GroupBy(s => new { s.Number, s.RType })
							.Select(g => new { Number = g.Key.Number, Occupied = (int)g.Key.RType + 1 - g.Count(p => p.P != null)});

							foreach (var r in availableRooms)
							{
								Console.WriteLine($"in room {r.Number} available {r.Occupied} places");
							}
						}

						break;
					case ConsoleKey.D5:
						{
							var occupiedSingleRooms = rooms
								.Where(r => r.Type == RoomType.SingleRoom)
								.Join(
									clients,
									r => r.Number,
									c => c.RoomNumber,
									(r, c) => new { Number = r.Number });

							Console.WriteLine($"occupied {occupiedSingleRooms.Count()} singlerooms");
							foreach (var r in occupiedSingleRooms)
							{
								Console.WriteLine($"Room {r.Number} is occupied");
							}
						}

						break;
					case ConsoleKey.D6:
						{
							var profit = clients
								.Join(
									rooms,
									c => c.RoomNumber,
									r => r.Number,
									(c, r) => new { Days = c.PaidDays, RType = r.Type, Cost = r.Cost })
								.Select(c => c.Days * c.Cost / ((int)c.RType + 1))
								.Sum();

							Console.WriteLine($"Total paid: {profit}$");
						}

						break;
					case ConsoleKey.D0:
					case ConsoleKey.Escape:
						exitFlag = true;
						InterfacePrinter.ClearConsole();
						break;
				}
			}
		}

		static void EditStaffMenuLoop()
		{
			InterfacePrinter.PrintStaffMenu();

			bool exitFlag = false;
			while (!exitFlag)
			{
				
				ConsoleKeyInfo pressedKey = Console.ReadKey(true);
				InterfacePrinter.ClearStaffMenuOutput();

				switch (pressedKey.Key)
				{
					case ConsoleKey.D1:
						PrintStaff();
						break;
					case ConsoleKey.D2:
						{
							String name = Console.ReadLine();
							if (name == null)
							{
								Console.WriteLine("Name cant be null");
								break;
							}
							var lastKey = staffMembers.Last().Key + 1;
							staffMembers.Add(new StaffMember(lastKey, name ?? "impossible"));
						}
						break;
					case ConsoleKey.D3:
						{ 
							var name = Console.ReadLine(); 
							if (name == null)
							{
								Console.WriteLine("Name cant be null");
								break;
							}
							if (staffMembers.Select(s => s.FullName).Contains(name) == false)
							{
								Console.WriteLine("There is no such staff with this name");
								break;
							}
							var index = staffMembers.FindIndex(m => m.FullName == name);
							staffMembers.RemoveAt(index);
						}

						break;
					case ConsoleKey.D0:
                    case ConsoleKey.Escape:
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
										schedule.Add(new KeyValuePair<int, Weekday>(staffToWeekdayKey, (Weekday)weekday));
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
					Console.WriteLine("Create file? (Enter)");
					if (Console.ReadKey(true).Key == ConsoleKey.Enter)
					{
						File.Create(path[i]);
					}
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

public static class LinqExtensions
{
    public static IEnumerable<TResult> LeftJoin<TLeft, TRight, TKey, TResult>(
        this IEnumerable<TLeft> left,
        IEnumerable<TRight> right,
        Func<TLeft, TKey> leftKeySelector,
        Func<TRight, TKey> rightKeySelector,
        Func<TLeft, TRight?, TResult> resultSelector)
    {
        return left
            .GroupJoin(
                right,
                leftKeySelector,
                rightKeySelector,
                (l, r) => new { l, r })
            .SelectMany(
                x => x.r.DefaultIfEmpty(),
                (x, r) => resultSelector(x.l, r));
    }
}