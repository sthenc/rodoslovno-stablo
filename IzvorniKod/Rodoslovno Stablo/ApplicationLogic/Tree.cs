using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public class Tree
	{
		// TODO, sve
		// XXX, pazi da je get public a set private

		public List<Person> osobe;
		public List<Connection> veze;


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
//		podaci Dohvati_podatke(Osoba) -- promijenjeno
//		bool Promijeni_podatke(Osoba, podaci) -- mijenjamo sa dohvati i postavi osobu
//		podaci_stabla Dohvati_podatke_stabla()  -- izbaceno, direktni pristup

		#region Upravljanje osobama
		public bool PersonExists(Guid ID)
		{
			return osobe.Exists(x => x.personID == ID);
		}

		public Person GetPersonByID(Guid ID)
		{
			return osobe.Single(x => x.personID == ID);
		}

		public void ChangePerson(Person osoba)
		{
			if (PersonExists(osoba.personID))
			{
				Person mijenjamo = GetPersonByID(osoba.personID);
				// makni staru osobu
				DeletePerson(mijenjamo.personID);
				
				// dodaj novu
				osobe.Add(osoba);
			}
			else
				throw new System.InvalidOperationException("ChangePerson: ID se ne smije dirati.");
		}

		public Guid AddPerson(string ime, string prezime)
		{ 
			Guid ID = new Guid();

			var osoba = new Person(ID, ime, prezime);

			osobe.Add(osoba);

			return ID;
		}

		public void DeletePerson(Guid ID)
		{
			if (PersonExists(ID))
			{
				osobe.RemoveAll(x => x.personID == ID);
			}
			else
				throw new System.InvalidOperationException("DeletePerson: Nema osobe sa tim ID-jem.");
		}
		#endregion

		#region Upravljanje vezama
		private bool ConnectionExists(Guid ID)
		{
			return veze.Exists(x => x.connectionID == ID);
		}

		public Connection GetConnectionByID(Guid ID)
		{
			return veze.Single(x => x.connectionID == ID);
		}

		public Guid AddConnection(Guid Osoba1, Guid Osoba2, string tip)
		{
			Guid ID = new Guid();

			var veza = new Connection(ID);

			if (PersonExists(Osoba1) && PersonExists(Osoba2))
			{
				veza.personID1 = Osoba1;
				veza.personID2 = Osoba2;
				veza.type = tip;
			}
			else
				throw new System.InvalidOperationException("Jedne od osoba nema u drvetu.");

			return ID;
		}

		public void DeleteConnection(Guid ID)
		{
			if (ConnectionExists(ID))
			{
				veze.RemoveAll(x => x.connectionID == ID);
			}
			else
				throw new System.InvalidOperationException("DeletePerson: Nema osobe sa tim ID-jem.");
		}
		#endregion

		#region Obiteljske operacije
		// za sada sve veze koje se dodaju u bazu moraju biti sacinjene od ovih osnovnih veza

		public Guid Add_Parent(Guid osoba1, Guid osoba2)
		{
			return AddConnection(osoba2, osoba1, "parent");
		}

		public Guid Add_Child(Guid osoba1, Guid osoba2)
		{
			return AddConnection(osoba1, osoba2, "parent");
		}

		public Guid Add_Partner(Guid osoba1, Guid osoba2)
		{
			return AddConnection(osoba1, osoba2, "partner");
		}

		// beware, LINQ dragons ahead
		public Guid[] Get_Parent(Guid osoba)
		{
			return veze.FindAll(x => x.personID2 == osoba && x.type.Equals("parent"))
						.Select(x => x.personID1)
						.ToArray();
		}

		public Guid[] Get_Child(Guid osoba)
		{
			return veze.FindAll(x => x.personID1 == osoba && x.type.Equals("parent"))
						.Select(x => x.personID2)
						.ToArray();
		}

		public Guid[] Get_Partner(Guid osoba)
		{
			return veze.FindAll(x => x.personID2 == osoba && x.type.Equals("partner"))
						.Select(x => x.personID1)
					.Concat(
						veze.FindAll(x => x.personID1 == osoba && x.type.Equals("partner"))
						.Select(x => x.personID2)
					).ToArray();
		}
		#endregion
	}
}
