using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		// Ovo tu će biti jedan prekrasan fajl sa puno koda

		// Trebamo implementirati funkcije za slijedeće odnose.
		//
		//partner/muž/žena
		//potomak/sin/kćer
		//roditelj/majka/otac
		// ----------------------------- ovo je praktički već napravljeno
		//brat/sestra
		//sestrična/bratić
		//baka/djed
		// ---------------------------- do ovdje apsolutno moramo imati xD
		//stric, strina, ujak, ujna, teta, tetak
		//punac/punica
		//svekar/svekrva
		//zet/nevjesta
		//šogor/šogorica
		// -------------------------- do ovdje je bitno
		//polubrat/polusestra -- ja bi ovo izbacio, polubrat je isto brat
		//potomak 2. koljeno/unuk/unuka
		//potomak 3. koljeno/praunuk/praunuka
		//roditelj 2. koljeno/prabaka/pradjed
		//roditelj 3. koljeno/šukunbaka/šukundjed
		

		// 
		public IEnumerable<Person> DohvatiRoditelje(Guid dijete)
		{
			return DohvatiOsobe(Drvo.GetParent(dijete));
		}


		public void DodajBaku(string[] parametri)
		{
			if (parametri.Length != 4)
				throw new System.ArgumentException();

			string unuk_ime = parametri[0];
			string unuk_prezime = parametri[1];

			string baka_ime = parametri[2];
			string baka_prezime = parametri[3];

			Guid unuk = NadjiOsobuPoImenu(unuk_ime, unuk_prezime, "Na kojeg unuka mislite ?");
			IEnumerable<Person> roditelji = DohvatiRoditelje(unuk);
			Guid roditelj;

			if (roditelji.Count() == 0)
			{
				roditelj = Drvo.AddPerson("N", "N");
				Drvo.AddParent(unuk, roditelj);
			}
			else if (roditelji.Count() > 1)
			{
				roditelj = QueryDisambiguator(roditelji, "Baka po kojem roditelju ?").ID;
			}
			else // if (roditelji.Count() == 1)
			{
				roditelj = roditelji.First().ID;
			}

			// ok, sada kada znamo na kojeg roditelja se misli

			Guid baka = DodajRoditelja(roditelj, baka_ime, baka_prezime);
		}

		private Guid DodajRoditelja(Guid dijete, string rod_ime, string rod_prezime)
		{
			Guid roditelj = Drvo.AddPerson(rod_ime, rod_prezime);

			Drvo.AddParent(dijete, roditelj);

			return roditelj;
		}

	//	public void 

		public void PromijeniPodatke(string[] parametri)
		{
			//TODO 
			//System.Console.WriteLine


			throw new System.NotImplementedException("TODO PromijeniPodatke");
		}

		public void IspisiDrvo(string[] parametri)
		{

			throw new System.NotImplementedException("TODO IspisiDrvo");
		}

		// definiraj novi exception, koristimo ga da program zna kada korisnik želi izaći iz programa
		public class QuitException : Exception { };

		public void Izlaz(string[] parametri)
		{
			throw new QuitException();
		}
	}
}
