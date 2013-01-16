using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public class UserManager
	{
		public bool Login(string username, string password)
		{
			// TODO, ovo i povezivanje sa bazom
			//throw new NotImplementedException();

			if (username == "admin" && password == "abc123")
				return true;

			return false;
		}

		public bool Logoff()
		{
			return true;
		}
	}
}
