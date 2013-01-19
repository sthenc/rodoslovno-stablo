using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ApplicationLogic
{
    [Serializable]
	public class Tree
	{
        [XmlElement("Person")]
		public List<Person> osobe;

        [XmlElement("Connection")]
		public List<Connection> veze;

        // Izvor http://csharpindepth.com/Articles/General/Singleton.aspx
        // ne treba nam thread-safety, nema filozofije
        private static Tree DrvoSingleton = null;

		private Tree() {}

        public static Tree GetInstance()
        {
            if (DrvoSingleton != null) return DrvoSingleton;

            DrvoSingleton = new Tree();

            DrvoSingleton.osobe = new List<Person>();
            DrvoSingleton.veze = new List<Connection>();

            return DrvoSingleton;
        }

//		Opći format funkcije je:
//		rezultat	Upit(Osoba1, Osoba2, parametri) {}
//			-- Osoba 1 je uvijek nadredjena osoba
//
//		Osnovne funkcije:
//		GUID Dodaj_osobu(ime)
//		bool Obrisi_osobu(Osoba)
//		GUID Dodaj_vezu(Osoba1, Osoba2, tip, ...)
//		bool Obrisi_vezu(Veza)
//		GUID Dodaj_roditelja(Osoba1, Osoba2)
//		GUID Dodaj_dijete(Osoba1, Osoba2)
//		GUID Dodaj_partnera(Osoba1, Osoba2)
//		[GUID] Dohvati_roditelja(Osoba)
//		[GUID] Dohvati_dijete(Osoba)
//		[GUID] Dohvati_partnera(Osoba)
//		podaci Dohvati_podatke(Osoba) -- zamijenjeno sa direktnim pristupom podatcima
//		bool Promijeni_podatke(Osoba, podaci) -- mijenjamo sa dohvati i postavi osobu
//		podaci_stabla Dohvati_podatke_stabla()  -- izbaceno, direktni pristup

		#region Upravljanje osobama
		public bool PersonExists(Guid ID)
		{
			return osobe.Exists(x => x.ID == ID);
		}

		public Person GetPersonByID(Guid ID)
		{
			return osobe.Single(x => x.ID == ID);
		}

		public IEnumerable<Person> GetPersonByID(IEnumerable<Guid> IDs)
		{
			return IDs.Select(ID => this.GetPersonByID(ID));
		}

		public void ChangePerson(Person osoba)
		{
			if (PersonExists(osoba.ID)) // pazi da bude isti ID
			{
				Person mijenjamo = GetPersonByID(osoba.ID);
				// makni staru osobu
				osobe.RemoveAll(x => x.ID == osoba.ID);
				
				// dodaj novu
				osobe.Add(osoba);
			}
			else
				throw new System.InvalidOperationException("ChangePerson: ID se ne smije dirati.");
		}

        public Guid AddPerson(string ime, string prezime)
        {
            Guid ID = Guid.NewGuid();

            var osoba = new Person(ID, ime, prezime);

            osobe.Add(osoba);

            return ID;
        }

        public Guid AddPerson(string ime, string prezime, DateTime datumRodjenja)
        {
            Guid ID = Guid.NewGuid();

            var osoba = new Person(ID, ime, prezime, datumRodjenja);

            osobe.Add(osoba);

            return ID;
        }

        public Guid AddPerson(string ime, string prezime, DateTime datumRodjenja, DateTime datumSmrti)
        {
            Guid ID = Guid.NewGuid();

            var osoba = new Person(ID, ime, prezime, datumRodjenja, datumSmrti);

            osobe.Add(osoba);

            return ID;
        }

		public void DeletePersonWithConnections(Guid ID)
		{
			if (PersonExists(ID))
			{
				osobe.RemoveAll(x => x.ID == ID);
				veze.RemoveAll(x => x.personID1 == ID || x.personID2 == ID);
			}
			else
				throw new System.InvalidOperationException("DeletePerson: Nema osobe sa tim ID-jem.");
		}
		#endregion

		#region Upravljanje vezama
		private bool ConnectionExists(Guid ID)
		{
			return veze.Exists(x => x.ID == ID);
		}

		public Connection GetConnectionByID(Guid ID)
		{
			return veze.Single(x => x.ID == ID);
		}

		public Guid AddConnection(Guid Osoba1, Guid Osoba2, string tip)
		{
			Guid ID = Guid.NewGuid();

			var veza = new Connection(ID);

			if (PersonExists(Osoba1) && PersonExists(Osoba2))
			{
				veza.personID1 = Osoba1;
				veza.personID2 = Osoba2;
				veza.type = tip;

				veze.Add(veza);
			}
			else
				throw new System.InvalidOperationException("Jedne od osoba nema u drvetu.");

			return ID;
		}

		public void DeleteConnection(Guid ID)
		{
			if (ConnectionExists(ID))
			{
				veze.RemoveAll(x => x.ID == ID);
			}
			else
				throw new System.InvalidOperationException("DeleteConnection: Nema veze sa tim ID-jem.");
		}
		#endregion

		#region Obiteljske operacije
		// za sada sve veze koje se dodaju u bazu moraju biti sacinjene od ovih osnovnih veza

		public Guid AddParent(Guid osoba1, Guid osoba2)
		{
			return AddConnection(osoba2, osoba1, "parent");
		}

		public Guid AddParent(Guid dijete, string rod_ime, string rod_prezime)
		{
			Guid roditelj = AddPerson(rod_ime, rod_prezime);

			AddParent(dijete, roditelj);

			return roditelj;
		}

		public Guid AddChild(Guid osoba1, Guid osoba2)
		{
			return AddConnection(osoba1, osoba2, "parent");
		}

		public Guid AddPartner(Guid osoba1, Guid osoba2)
		{
			return AddConnection(osoba1, osoba2, "partner");
		}

		// beware, LINQ dragons ahead
			// beware, LINQ dragons ahead
		public IEnumerable<Guid> GetParent(Guid osoba)
		{
			return veze.FindAll(x => x.personID2 == osoba && x.type.Equals("parent"))
			.Select(x => x.personID1);
		}

		public IEnumerable<Person> GetParents(Guid dijete)
		{
			return GetPersonByID(GetParent(dijete));
		}
		
		public IEnumerable<Guid> GetChild(Guid osoba)
		{
			return veze.FindAll(x => x.personID1 == osoba && x.type.Equals("parent"))
			.Select(x => x.personID2);
		}

		public IEnumerable<Person> GetChildren(Guid roditelj)
		{
			return GetPersonByID(GetChild(roditelj));
		}
		
		public IEnumerable<Guid> GetPartner(Guid osoba)
		{
			return veze.FindAll(x => x.personID2 == osoba && x.type.Equals("partner"))
					.Select(x => x.personID1)
				.Concat(
					veze.FindAll(x => x.personID1 == osoba && x.type.Equals("partner"))
					.Select(x => x.personID2)
				);
		}

		public IEnumerable<Person> GetPartners(Guid part)
		{
			return GetPersonByID(GetPartner(part));
		}
		#endregion

        #region Serijalizacija

        public void clearTree() {
            osobe.RemoveAll(x => true);
            veze.RemoveAll(x =>true);

        
        }
        public void Save(string path)
        {
            XmlSerializer xmlWriter = new XmlSerializer(typeof(Tree));
            TextWriter outputFile = new StreamWriter(path);

            xmlWriter.Serialize(outputFile, this);

            outputFile.Close();
        }

        public void Save(Stream file)
        {
            XmlSerializer xmlWriter = new XmlSerializer(typeof(Tree));
            xmlWriter.Serialize(file, this);
        }

        public static Tree Load(string path)
        {
            XmlSerializer xmlReader = new XmlSerializer(typeof(Tree));
            TextReader inputFile = new StreamReader(path);

            Tree deserializirano = (Tree)xmlReader.Deserialize(inputFile);
            Tree staro = Tree.GetInstance();

            staro.osobe = deserializirano.osobe;
            staro.veze = deserializirano.veze;

            return staro;
        }

		// malo modificirano tako da mozemo unit testat
		public string SaveTest()
		{
			XmlSerializer xmlWriter = new XmlSerializer(typeof(Tree));
			TextWriter output = new StringWriter();

			xmlWriter.Serialize(output, this);

			return output.ToString();
		}

		public static Tree LoadTest(string data)
		{
			XmlSerializer xmlReader = new XmlSerializer(typeof(Tree));
			TextReader input = new StringReader(data);

            Tree deserializirano = (Tree)xmlReader.Deserialize(input);
            Tree staro = Tree.GetInstance();

            staro.osobe = deserializirano.osobe;
            staro.veze = deserializirano.veze;

            return staro;
		}

        #endregion
	}
}
