using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

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

	[Serializable]
    public class Person
	{
        [XmlElement("ID")]
		public Guid ID { get; set; }

        [XmlElement("name")]
		public string name { get; set; }

        [XmlElement("surname")]
		public string surname { get; set; }

        [XmlElement("birthDate")]
		public DateTime birthDate { get; set; }

        [XmlElement("photo")]
		public Image photo { get; set; } // TODO

<<<<<<< HEAD
        [XmlElement("adress")]
=======
        [XmlElement("address")]
>>>>>>> origin/marko_dev
		public string adress { get; set; } // mozda da ovo u klasu prebacimo?

        [XmlElement("CV")]
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
