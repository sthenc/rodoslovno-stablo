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
		// ovo je jedina funkcija koju mozemo koristiti za to
		private Func<string> GetLine;

		// tablica sa mapiranjem komandi i kljucnih rijeci, zajedno sa opisima
		private List<CommandDescriptor> komande;

		public QueryProcessor(Tree drvo, Func<List<Person>, Person> QD, 
								Func<string> daj_liniju, TextWriter tw = null)
		{
			// dodajmo malo couplinga
			Drvo = drvo;
			QueryDisambiguator = QD;
			GetLine = daj_liniju;

			// preusmjeri System.Console.Out tamo gdje nam odgovara da mozemo koristiti System.Console.WriteLine
			if (tw != null)		// Da omogucimo ovo http://saezndaree.wordpress.com/2009/03/29/how-to-redirect-the-consoles-output-to-a-textbox-in-c/
				System.Console.SetOut(tw);

			Initialize_Commands();
		}

		public void Initialize_Commands()
		{
			komande = new List<CommandDescriptor>();
			komande.Add(new CommandDescriptor("dodaj_baku", DodajBaku, "dodaj_baku ime, [prezime]"));
			komande.Add(new CommandDescriptor("promijeni_podatke", PromijeniPodatke, "promijeni_podatke ime, [prezime]"));
			komande.Add(new CommandDescriptor("izlaz", Izlaz));

		}

		#endregion

		#region Textualno sucelje
		public void RunCommand(string input)
		{
			// Parsiranje upita
			// format upita je  "upit argument1, argument2,argument3,    argument4"

			// prvo razdvoji argumente od imena upita
			char[] delimiters = new char[]{' ', '\t', '\n'};
			
			input += "\n"; // za upite bez argumenata

			int index = input.IndexOfAny(delimiters);

			string upit = input.Substring(0, index).Trim();
			
			string argumenti = input.Substring(index).Trim();
			
			// onda razdvoji pojedinacne argumente
			string[] args = argumenti.Split(',');

			// izbaci spejseve sa pocetka i kraja
			for (int i = 0; i < args.Length; ++i)
			{
				args[i] = args[i].Trim();
			}

			bool komanda_nadjena = false;
			foreach (var cd in komande)
			{
				// nadji odgovarajucu funkciju
				if (cd.keywords.Exists(x => x.Equals(upit)))
				{
					komanda_nadjena = true;
					// i pozovi ju
					try
					{
						cd.func(args);
					}
					catch (System.InvalidOperationException e)
					{
						System.Console.WriteLine("Nemoguća operacija: {0}\n", e.Message);
					}
					catch (System.ArgumentException e)
					{
						System.Console.WriteLine("Pogrešno oblikovan upit: {0}\n\nIspravno korištenje:\n {1}\n", 
													e.Message, cd.description);
					}
				}
			}

			if (!komanda_nadjena)
			{
				IspisiNaredbe();
			}
		}

		private void IspisiNaredbe()
		{
			System.Console.WriteLine("Program podržava slijedeće operacije: \n");

			foreach (var cd in komande)
			{
				System.Console.WriteLine(cd.description);
			}
		}

		#endregion

		#region Implementacija komandi (jeej)

		public void DodajBaku(string[] parametri)
		{
			throw new System.NotImplementedException("TODO DodajBaku");
		}

		public void PromijeniPodatke(string[] parametri)
		{
			throw new System.NotImplementedException("TODO PromijeniPodatke");
		}

		public void Izlaz(string[] parametri)
		{
			throw new QuitException();
		}
		#endregion

		#region Pomocni tipovi
		public class QuitException : Exception { };

		private class CommandDescriptor
		{
			public List<string> keywords { get; set; }
			public string description { get; set; }
			public Action<string[]> func { get; set;} 

			public CommandDescriptor(string[] words, Action<string[]> funk, string desc = null)
			{
				Initialize(words, funk, desc);
			}

			public CommandDescriptor(string word, Action<string[]> funk, string desc = null) 
			{
				Initialize(new string[] { word }, funk, desc);
			}
			
			public void Initialize(string[] words, Action<string[]> funk, string desc)
			{ 
				keywords = new List<string>();
				keywords.AddRange(words);

				func = funk;
				description = desc;

				//System.Console.WriteLine("{0} kljucnih rijeci", keywords.Count);
			}
		}
		#endregion
	}
}
