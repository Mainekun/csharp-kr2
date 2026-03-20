using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kr2.Roles
{
    internal class StaffMember
    {
		private int _key;
		private String _fullName;
		//private List<int> _servingFloors;
		//private List<Weekday> _workingDays;

		public String FullName { get { return _fullName; } }
		public int Key { get { return _key; } }

		public StaffMember()
		{
			_key = -1;
			_fullName = string.Empty;
			//_servingFloors = new List<int>();
			//_workingDays = new List<Weekday>();
		}

		public StaffMember(int key, string fullName)
		{
			_key = key;
			_fullName = fullName;
		}

        public override string ToString()
        {
			return $"Staff member\n" +
				$"Name {_fullName}";
        }

		public string ToLineString()
		{
			return $"{_key};{_fullName}\n";
		}
    }

	public enum Weekday
	{
		Monday,
		Tuesday,
		Wednesday,
		Thursday,
		Friday,
		Saturday,
		Sunday
	}
}
