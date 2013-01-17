using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationLogic
{
	public partial class QueryProcessor
	{
		// pomoćne funkcije

		public class PersonNotFoundException : Exception
		{
			public PersonNotFoundException(string msg) : base(msg) { }
		}

        public Guid FindPersonByName(string ime, string prezime, string pitanje = "Na koga mislite ?")
		{ 
			List<Person> kandidati = Drvo.osobe.FindAll(x => x.name == ime && x.surname == prezime);

			Person pobjednik = null;
			if (kandidati.Count > 1)
			{
				pobjednik = QueryDisambiguator(kandidati, pitanje);
			}
			else if (kandidati.Count == 1)
			{
				pobjednik = kandidati.First();
			}

			if (pobjednik == null)
				throw new PersonNotFoundException(String.Format("Ne mogu pronaći osobu {0} {1}", ime,prezime));
			
			return pobjednik.ID;
		}

		public void PrintPerson(Guid osoba)
		{
			Person persona = Drvo.GetPersonByID(osoba);
			PrintPerson(persona);
		}

		public void PrintPerson(Person osoba)
		{
			System.Console.WriteLine(osoba);
		}

		public void PrintPersons(IEnumerable<Guid> osobe)
		{
			IEnumerable<Person> persone = osobe.Select(o => Drvo.GetPersonByID(o));
			PrintPersons(persone);
		}

		public void PrintPersons(IEnumerable<Person> osobe)
		{
			foreach (var o in osobe)
			{
				System.Console.WriteLine(o);
			}
			//throw new System.NotImplementedException();
		}
	}
}
