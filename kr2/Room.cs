using kr2.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kr2
{
	internal class Room
	{
		private int _number;
		private int _floor;
		private RoomType _type;
		private int _cost;

		public Room()
		{
			_number = -1;
			_floor = -1;
			_type = RoomType.SingleRoom;
			_cost = -1;
		}

		public Room(int number, int floor, RoomType type, int cost)
		{
			_number = number;
			_floor = floor;
			_type = type;
			_cost = cost;
		}

        public override string ToString()
        {
            return $"Room {_number} on {_floor} floor, {_type.ToString()}, {_cost}$";
        }
	}

	internal enum RoomType
	{
		SingleRoom,
		TwinRoom,
		TripleRoom
	}
}
