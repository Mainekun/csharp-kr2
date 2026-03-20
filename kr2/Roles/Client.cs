using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kr2.Roles
{
    internal class Client
    {
		private int _key;
		private String _fullName;
		private String _id;
		private String _whence; // From where.
		private int _number;
		private int _place;
		private DateOnly _arrivialDate;
		private int _paidDays;

		public Client()
		{
			_key = -1;
			_fullName = String.Empty;
			_id = String.Empty;
			_whence = String.Empty;
			_number = -1;
			_place = -1;
			_arrivialDate = new DateOnly(1911, 10, 10);
			_paidDays = -1;
		}

		public Client(int key, string fullName, string id, string whence, int number, int place, DateOnly arrivialDate, int paidDays)
        {
            _key = key;
            _fullName = fullName;
            _id = id;
            _whence = whence;
            _number = number;
            _place = place;
            _arrivialDate = arrivialDate;
            _paidDays = paidDays;
        }

        public override string ToString()
        {
			return $"Client " +
				$"{_fullName}\n" +
				$"Personal ID: {_id}\n" +
				$"Whemce: {_whence}\n" +
				$"Paid number: {_number}\n" +
				$"Place: {_place}\n" +
				$"Arrivied in: {_arrivialDate.ToLongDateString()}\n" +
				$"Paid days: {_paidDays}";
        }

		public string ToLineString()
		{
			return $"{_key};{_fullName};{_id};{_whence};{_number};{_place};{_arrivialDate.ToString("dd/MM/yyyy")};{_paidDays};\n";
		}
	}
}
