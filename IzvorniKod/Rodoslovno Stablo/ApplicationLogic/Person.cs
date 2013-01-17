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

        [XmlElement("deathDate")]
        public DateTime deathDate { get; set; }

        [XmlElement("photo")]
		public Image photo { get; set; } // TODO

        [XmlElement("address")]
		public string address { get; set; } // mozda da ovo u klasu prebacimo?

        [XmlElement("CV")]
        public string CV { get; set; }

        [XmlElement("sex")]
        public Sex sex;

		public Person() { }

		public Person(Guid id, string ime, string prezime)
		{
			ID = id;
			name = ime;
			surname = prezime;
			photo = null;
			birthDate = new DateTime(1000, 1, 1);
			address = "";
			CV = "";
		}

        public enum Sex { [XmlEnum("Male")] Male, [XmlEnum("Female")] Female, [XmlEnum("Unknown")] Unknown }; // mogli bismo i nastaviti

		public override string ToString()
		{	// TODO
			return String.Format(
				"ID = {0}, ime = {1}, prezime = {2}, spol = {3}, datum_rodjenja = {4}, adresa = {5}, CV = {6}, Fotka = {7}", 
				ID, name, surname, sex, birthDate, address, CV, photo);
		}

		// Izvor http://msdn.microsoft.com/en-us/library/ms173147%28v=vs.80%29.aspx
		public override bool Equals(System.Object obj)
		{
			// ako je null nije jednako
			if (obj == null)
			{
				return false;
			}

			// Ako se ne moze kastati sigurno nije dobro
			Person p = obj as Person;
			if ((System.Object)p == null)
			{
				return false;
			}

			return this.Equals(p);
		}

		public bool Equals(Person p)
		{
			return ID == p.ID && name == p.name && surname == p.surname && photo == p.photo && sex == p.sex && birthDate == p.birthDate && address == p.address;
		}
	}
}
