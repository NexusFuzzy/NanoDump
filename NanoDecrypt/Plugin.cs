using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoDecrypt
{
    class Plugin
    {
		private Guid _Guid;
		public Guid Guid
		{
			get { return _Guid; }
			set { _Guid = value; }
		}

		private DateTime _LastUpdated;
		public DateTime LastUpdated
		{
			get { return _LastUpdated; }
			set { _LastUpdated = value; }
		}

		private string _Name;
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		private byte[] _Payload;
		public byte[] Payload
		{
			get { return _Payload; }
			set { _Payload = value; }
		}
	}
}
