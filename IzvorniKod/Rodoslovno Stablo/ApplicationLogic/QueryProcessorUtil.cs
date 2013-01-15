using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		// pomoćne funkcije

		public class PersonNotFoundException : Exception
		{
			public PersonNotFoundException(string msg) : base(msg) { }
		}

		Guid NadjiOsobuPoImenu(string ime, string prezime)
		{ 
			List<Person> kandidati = Drvo.osobe.FindAll(x => x.name == ime && x.surname == prezime);

			Person pobjednik = null;

			if (pobjednik == null)
				throw new PersonNotFoundException(String.Format("Ne mogu pronaći osobu {0} {1}", ime,prezime));
			
			return pobjednik.ID;
		}
	}
}
