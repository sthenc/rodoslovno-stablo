using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Drawing.Imaging;

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
//  http://stackoverflow.com/questions/1907077/serialize-a-bitmap-in-c-net-to-xml serijalizacija slike

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

        [XmlIgnore]
		public Image photo { get; set; } // TODO

        [XmlElement("address")]
		public string address { get; set; } // mozda da ovo u klasu prebacimo?

        [XmlElement("CV")]
        public string CV { get; set; }

        [XmlElement("sex")]
        public Sex sex;

        [XmlElement("telephone")]
        public string telephone  { get; set; }

        [XmlElement("email")]
        public string email { get; set; }

        [XmlElement("positionX")]
        public int positionX { get; set; }

        [XmlElement("positionY")]
        public int positionY{ get; set; }


        public readonly static DateTime nedefiniranDatum = new DateTime(1000, 1, 1);

        public Person() { }

        public Person(Guid id, string ime, string prezime) : this(id, ime, prezime, nedefiniranDatum, nedefiniranDatum) { }

        public Person(Guid id, string ime, string prezime, DateTime datumRodenja) : this(id, ime, prezime, datumRodenja, nedefiniranDatum) { }

		public Person(Guid id, string ime, string prezime, DateTime datumRodenja, DateTime datumSmrti)
		{
			ID = id;
			name = ime;
			surname = prezime;
			photo = null;
            birthDate = datumRodenja;
            deathDate = datumSmrti;
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
			return ID == p.ID && name == p.name && surname == p.surname && photo == p.photo && sex == p.sex && birthDate == p.birthDate && address == p.address
                && positionX==p.positionX && positionY==p.positionY;
		}

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("Photo")]
        public byte[] LargeIconSerialized
        {
            get
            { // serialize
                if (photo == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    photo.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    photo = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        photo = Image.FromStream(ms);
                    }
                }
            }
        }
	}

}
