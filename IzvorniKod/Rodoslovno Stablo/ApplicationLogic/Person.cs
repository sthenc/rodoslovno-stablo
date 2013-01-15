using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ApplicationLogic
{

//  OSOBA
//
//	osobaID
//	osobaIme
//	osobaPrezime
//	datRod
//	foto
//	adresa
//	zivotopis
//	K = {osobaID}

	public class Person
	{
		public Guid ID { get; set; }
		public string name { get; set; }
		public string surname { get; set; }
		public DateTime birthDate { get; set; }
		public Image photo { get; set; } // TODO
		public string adress { get; set; } // mozda da ovo u klasu prebacimo?
		public string CV { get; set; }
		public Sex sex;

		public Person(Guid id, string ime, string prezime)
		{
			ID = id;
			name = ime;
			surname = prezime;
			sex = Sex.Unknown;
		}

		public enum Sex {Male, Female, Unknown}; // mogli bismo i nastaviti
	}
}	
