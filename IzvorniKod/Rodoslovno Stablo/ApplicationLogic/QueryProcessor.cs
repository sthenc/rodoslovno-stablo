using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ApplicationLogic
{
	public class QueryProcessor
	{
		#region Inicijalizacija
		private Tree Drvo;
		
		// Podesiva funkcija za odlucivanje o kojoj osobi se radi kada je upit dvosmislen
		private Func<List<Person>, Person> QueryDisambiguator;
		// Podesiva funkcija za dohvacanje korisnikovog unosa
		private Func<string> GetLine;

		// tablica sa mapiranjem komandi i kljucnih rijeci, zajedno sa opisima
		private List<CommandDescriptor> komande;

		public QueryProcessor(Tree drvo, Func<List<Person>, Person> QD, 
								Func<string> daj_liniju, TextWriter tw = null)
		{
			// dodajmo malo couplinga
			Drvo = drvo;
			QueryDisambiguator = QD;

			if (tw != null)		// Da omogucimo ovo http://saezndaree.wordpress.com/2009/03/29/how-to-redirect-the-consoles-output-to-a-textbox-in-c/
				System.Console.SetOut(tw);
		}


		#endregion
		public void RunCommand(string input)
		{
			// TODO parsaj upit
			throw new QuitException();
		}

		#region Implementacija komandi (jeej)

		#endregion

		#region Pomocni tipovi
		public class QuitException : Exception { };

		private class CommandDescriptor
		{
			public List<string> keywords;
			public string description;
			public Func<List<string>> func;

		}
		#endregion
	}
}
