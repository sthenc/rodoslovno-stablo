using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		// Trebamo implementirati funkcije za slijedeće odnose.
		//
		//osobu
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


		public void AddGrandsomething(string[] parametri, Person.Sex spol = Person.Sex.Unknown)
		{
			if (parametri.Length != 4)
				throw new System.ArgumentException();

			string unuk_ime = parametri[0];
			string unuk_prezime = parametri[1];

			string baka_ime = parametri[2];
			string baka_prezime = parametri[3];

			Guid unuk = FindPersonByName(unuk_ime, unuk_prezime, "Na kojeg unuka mislite ?");
			IEnumerable<Person> roditelji = Drvo.GetParents(unuk);
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

			Guid baka = Drvo.AddParent(roditelj, baka_ime, baka_prezime);
			Person nona = Drvo.GetPersonByID(baka);
			nona.sex = spol;
			Drvo.ChangePerson(nona);
		}

		public void AddGrandmother(string[] parametri)
		{
			AddGrandsomething(parametri, Person.Sex.Female);
		}

		public void AddGrandfather(string[] parametri)
		{
			AddGrandsomething(parametri, Person.Sex.Male);
		}

		public void AddGrandparent(string[] parametri)
		{
			AddGrandsomething(parametri, Person.Sex.Unknown);
		}

	//	public void 

		public void ChangeData(string[] parametri)
		{
			//TODO 
			//System.Console.WriteLine


			throw new System.NotImplementedException("TODO PromijeniPodatke");
		}

		public void PrintTree(string[] parametri)
		{

			throw new System.NotImplementedException("TODO IspisiDrvo");
		}

		public void PrintPerson(string[] parametri)
		{
			throw new System.NotImplementedException("TODO PrintPerson");
		}

		// definiraj novi exception, koristimo ga da program zna kada korisnik želi izaći iz programa
		public class QuitException : Exception { };

		public void Quit(string[] parametri)
		{
			throw new QuitException();
		}
	}
}
