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

        [XmlElement("address")]
		public string adress { get; set; } // mozda da ovo u klasu prebacimo?

        [XmlElement("CV")]
        public string CV { get; set; }

        [XmlElement("sex")]
        public Sex sex;

		public Person(Guid id, string ime, string prezime)
		{
			ID = id;
			name = ime;
			surname = prezime;
		}

        public enum Sex { [XmlEnum("Male")] Male, [XmlEnum("Female")] Female, [XmlEnum("Unknown")] Unknown }; // mogli bismo i nastaviti

		public override string ToString()
		{	// TODO
			return String.Format("ID = {0}, ime = {1}, prezime = {2}, spol = {3}", ID, name, surname, sex);
		}
	}
}
