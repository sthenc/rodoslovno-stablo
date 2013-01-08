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
		public Guid personID { get; set; }
		public string personName { get; set; }
		public string personSurname { get; set; }
		public DateTime birthDate { get; set; }
		public Image photo { get; set; } // TODO
		public string adress { get; set; } // mozda da ovo u klasu prebacimo?
		public string CV { get; set; }

		public Person(Guid ID, string name, string surname)
		{
			personID = ID;
			personName = name;
			personSurname = surname;
		}
	}
}
