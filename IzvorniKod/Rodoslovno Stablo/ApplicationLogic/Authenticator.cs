using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public class Authenticator
	{
		public bool Authorize(string username, string password)
		{
			// TODO, ovo i povezivanje sa bazom
			//throw new NotImplementedException();

			if (username == "admin" && password == "abc123")
				return true;

			return false;
		}
	}
}
